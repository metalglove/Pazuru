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
            CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            Task<SolvedPuzzlesDto> getPuzzlesTask =
                Task.Run(async () => await _puzzleStorageService.GetPreviouslySolvedPuzzles(), cts.Token)
                    .ContinueWith(task =>
                        {
                            if (task.IsCanceled || task.IsFaulted)
                                return default;
                            return task.Result;
                        }, 
                        TaskContinuationOptions.None);

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
            string puzzleStateAsString = puzzle.PuzzleState.ToString();
            SudokuGeneratePuzzleResponse generatedSudokuPuzzleMessage = new SudokuGeneratePuzzleResponse
            {
                Data = new SudokuPuzzleState
                {
                    PuzzleAsString = puzzleStateAsString
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

            SudokuPuzzle puzzle = new SudokuPuzzle(new PuzzleState(Encoding.Default.GetBytes(sudokuPuzzleStateMessage.Data.PuzzleAsString)));
            PuzzleSolveDto solve = _sudokuPuzzleService.Solve(puzzle);
            if (!solve.Success)
            {
                SudokuStateChangeEvent msg = new SudokuStateChangeEvent
                {
                    EventName = "sudokuPuzzleStateChange",
                    Data = default,
                    Message = "Puzzle state is not solvable",
                    Success = false
                };
                await SendMessageAsync(webSocket, JsonConvert.SerializeObject(msg));
                return;
            }
            string currentState = solve.PuzzleStates.First().ToString();
            foreach (PuzzleState state in solve.PuzzleStates.Skip(1).SkipLast(1))
            {
                SudokuStateChange eChangeEvent = GetSudokuStateChangeEvent(currentState, state.ToString());
                SudokuStateChangeEvent msg = new SudokuStateChangeEvent
                {
                    EventName = "sudokuPuzzleStateChange",
                    Data = eChangeEvent,
                    Message = "",
                    Success = true
                };
                currentState = state.ToString();
                await SendMessageAsync(webSocket, JsonConvert.SerializeObject(msg));
            }

            PuzzleState puzzleState = solve.PuzzleStates.Last();
            PuzzleDto puzzleDto = new PuzzleDto
            {
                OriginalPuzzle = sudokuPuzzleStateMessage.Data.PuzzleAsString,
                SolvedPuzzle = puzzleState.ToString(),
                PuzzleType = "Sudoku"
            };
            _ = _puzzleStorageService.SavePuzzle(puzzleDto);

            SudokuStateChange changeEvent = GetSudokuStateChangeEvent(currentState, puzzleState.ToString());
            changeEvent.LastEvent = true;
            SudokuStateChangeEvent msg2 = new SudokuStateChangeEvent
            {
                EventName = "sudokuPuzzleStateChange",
                Data = changeEvent,
                Message = "",
                Success = true
            };
            await SendMessageAsync(webSocket, JsonConvert.SerializeObject(msg2));
        }
        private static SudokuStateChange GetSudokuStateChangeEvent(string a, string b)
        {
            for (int i = 0; i < a.Length - 1; i++)
            {
                if (a[i] != b[i])
                    return new SudokuStateChange { Changed = true, Index = i, NumberBefore = a[i] - 48, NumberAfter = b[i] - 48, LastEvent = false };
            }

            return new SudokuStateChange { Changed = true, Index = 80, NumberBefore = a[80] - 48, NumberAfter = b[80] - 48, LastEvent = true };
        }
    }
}
