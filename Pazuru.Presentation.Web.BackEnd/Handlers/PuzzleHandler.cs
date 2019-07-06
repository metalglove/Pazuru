using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Pazuru.Application.DTOs;
using Pazuru.Application.Interfaces;
using Pazuru.Domain;
using Pazuru.Presentation.Web.BackEnd.Events;
using Pazuru.Presentation.Web.BackEnd.Models;
using Pazuru.Presentation.Web.BackEnd.Requests;
using Pazuru.Presentation.Web.BackEnd.Responses;
using Pazuru.Presentation.Web.BackEnd.Utilities;
using Pazuru.Sudoku;

namespace Pazuru.Presentation.Web.BackEnd.Handlers
{
    public sealed class PuzzleHandler : WebSocketHandler
    {
        private readonly IPuzzleService<SudokuPuzzle> _sudokuPuzzleService;
        private readonly IPuzzleStorageService _puzzleStorageService;

        public PuzzleHandler(
            WebSocketConnectionManager webSocketConnectionManager, 
            IPuzzleService<SudokuPuzzle> sudokuPuzzleService,
            IPuzzleStorageService puzzleStorageService) : base(webSocketConnectionManager)
        {
            _sudokuPuzzleService = sudokuPuzzleService;
            _puzzleStorageService = puzzleStorageService;
        }

        public override Task ReceiveAsync(IWebSocket socket, byte[] buffer)
        {
            string json = Encoding.UTF8.GetString(buffer);
            PreMessage preMessage = JsonConvert.DeserializeObject<PreMessage>(json);
            switch (preMessage.EventName)
            {
                case "sudokuSolvePuzzleRequest":
                    return SudokuSolvePuzzleRequest(socket, json);
                case "sudokuGeneratePuzzleRequest":
                    return SudokuGeneratePuzzleRequest(socket);
                case "previouslySolvedPuzzlesRequest":
                    return PreviouslySolvedPuzzlesRequest(socket);
                case "sudokuVerifyRequest":
                    return SudokuVerifyPuzzleRequest(socket, json);
                default:
                    return Task.CompletedTask;
            }
        }
        private async Task SudokuVerifyPuzzleRequest(IWebSocket socket, string json)
        {
            VerifySudokuRequest sudokuPuzzleStateMessage = JsonConvert.DeserializeObject<VerifySudokuRequest>(json);
            SudokuPuzzle puzzle = new SudokuPuzzle(new PuzzleState(Encoding.Default.GetBytes(sudokuPuzzleStateMessage.Data.PuzzleAsString)));
            PuzzleSolveDto solve = _sudokuPuzzleService.Solve(puzzle);
            VerifySudokuPuzzleResponse verifySudokuEvenMessage;
            if (!solve.Success)
            {
                verifySudokuEvenMessage = new VerifySudokuPuzzleResponse
                {
                    Data = default,
                    EventName = "sudokuVerifyRequest",
                    Message = "Failed to verify puzzle.",
                    Success = false
                };
            }
            else
            {
                string solvedState = solve.SolvedState.ToString();
                List<int> correctIndexes = new List<int>();
                for (int i = 0; i < sudokuPuzzleStateMessage.Data.PuzzleAsString.Length; i++)
                {
                    if (sudokuPuzzleStateMessage.Data.CurrentPuzzle[i] == solvedState[i])
                        correctIndexes.Add(i);
                }

                verifySudokuEvenMessage = new VerifySudokuPuzzleResponse
                {
                    Data = new VerifiedSudokuState
                    {
                        CorrectIndexes = correctIndexes.ToArray()
                    },
                    EventName = "sudokuVerifyRequest",
                    Message = "",
                    Success = true
                };
            }
            
            await SendMessageAsync(socket, JsonConvert.SerializeObject(verifySudokuEvenMessage));
        }
        private async Task PreviouslySolvedPuzzlesRequest(IWebSocket webSocket)
        {
            Task<SolvedPuzzlesDto> getPuzzlesTask;
            using (CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(5)))
            {
                getPuzzlesTask = Task.Run(async () => await _puzzleStorageService.GetPreviouslySolvedPuzzles(), cts.Token)
                    .ContinueWith(task =>
                    {
                        if (task.IsCanceled || task.IsFaulted)
                            return default;
                        return task.Result;
                    }, TaskContinuationOptions.None);
            }

            SolvedPuzzlesDto previouslySolvedPuzzles = await getPuzzlesTask;
            bool success = previouslySolvedPuzzles != default(SolvedPuzzlesDto);

            PreviouslySolvedPuzzlesResponse previouslySolvedPuzzlesMessage = new PreviouslySolvedPuzzlesResponse
            {
                Data = previouslySolvedPuzzles,
                EventName = "previouslySolvedPuzzlesRequest",
                Message = success ? "" : "Failed to get a response from the rest api.",
                Success = success
            };
        
            await SendMessageAsync(webSocket, JsonConvert.SerializeObject(previouslySolvedPuzzlesMessage));
        }
        private async Task SudokuGeneratePuzzleRequest(IWebSocket webSocket)
        {
            SudokuPuzzle puzzle = _sudokuPuzzleService.Generate();
            SudokuGeneratePuzzleResponse generatedSudokuPuzzleMessage = new SudokuGeneratePuzzleResponse
            {
                Data = new SudokuPuzzleState
                {
                    PuzzleAsString = puzzle.PuzzleState.ToString()
                },
                EventName = "sudokuGeneratePuzzleRequest",
                Message = "",
                Success = true
            };

            await SendMessageAsync(webSocket, JsonConvert.SerializeObject(generatedSudokuPuzzleMessage));
        }
        private async Task SudokuSolvePuzzleRequest(IWebSocket webSocket, string json)
        {
            SolveSudokuRequest sudokuPuzzleStateMessage = JsonConvert.DeserializeObject<SolveSudokuRequest>(json);
            SudokuStateChangeEvent sudokuStateChangeEvent = new SudokuStateChangeEvent { Data = default, Message = "" };

            if (sudokuPuzzleStateMessage.Data.PuzzleAsString.Length != 81 ||
                sudokuPuzzleStateMessage.Data.PuzzleAsString.Any(c => !char.IsDigit(c)))
            {
                sudokuStateChangeEvent.Message = "Faulty puzzle state.";
                await SendMessageAsync(webSocket, JsonConvert.SerializeObject(sudokuStateChangeEvent));
                return;
            }

            SudokuPuzzle puzzle = new SudokuPuzzle(new PuzzleState(Encoding.Default.GetBytes(sudokuPuzzleStateMessage.Data.PuzzleAsString)));
            PuzzleSolveDto solve = _sudokuPuzzleService.Solve(puzzle);
            if (!solve.Success)
            {
                sudokuStateChangeEvent.Message = "Puzzle state is not solvable.";
                await SendMessageAsync(webSocket, JsonConvert.SerializeObject(sudokuStateChangeEvent));
                return;
            }

            sudokuStateChangeEvent.Success = true;
            string currentState = solve.PuzzleStates.First().ToString();
            foreach (PuzzleState state in solve.PuzzleStates.Skip(1).SkipLast(1))
            {
                SudokuStateChange eChangeEvent = GetSudokuStateChange(currentState, state.ToString());
                if (eChangeEvent == null)
                    break;
                sudokuStateChangeEvent.Data = eChangeEvent;
                currentState = state.ToString();
                await SendMessageAsync(webSocket, JsonConvert.SerializeObject(sudokuStateChangeEvent));
            }

            PuzzleState puzzleState = solve.PuzzleStates.Last();
            string puzzleStateAsString = puzzleState.ToString();
            PuzzleDto puzzleDto = new PuzzleDto
            {
                OriginalPuzzle = sudokuPuzzleStateMessage.Data.PuzzleAsString,
                SolvedPuzzle = puzzleStateAsString,
                PuzzleType = "Sudoku"
            };
            // NOTE: This just runs a puzzle save attempt.
            // NOTE: We do not care if it fails or passes, that is why we discard the task.
            _ = _puzzleStorageService.SavePuzzle(puzzleDto);

            sudokuStateChangeEvent.Data = CreateSudokuStateChangeFromIndex(80, currentState, puzzleStateAsString, true);
            await SendMessageAsync(webSocket, JsonConvert.SerializeObject(sudokuStateChangeEvent));
        }
        private static SudokuStateChange GetSudokuStateChange(string a, string b)
        {
            SudokuStateChange sudokuStateChange = null;
            for (int i = 0; i < a.Length - 1; i++)
            {
                if (a[i] == b[i])
                    continue;
                sudokuStateChange = CreateSudokuStateChangeFromIndex(i, a, b);
                break;
            }
            return sudokuStateChange;
        }
        private static SudokuStateChange CreateSudokuStateChangeFromIndex(int index, string a, string b, bool lastEvent = false)
        {
            return new SudokuStateChange
            {
                Changed = true,
                Index = index,
                NumberBefore = a[index] - 48,
                NumberAfter = b[index] - 48,
                LastEvent = lastEvent
            };
        }
    }
}
