using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Pazuru.Application.DTOs;
using Pazuru.Application.Interfaces;
using Pazuru.Domain;
using Pazuru.Sudoku;

namespace Pazuru.Presentation.Web.BackEnd.Handlers
{
    public class PuzzleHandler : WebSocketHandler
    {
        private readonly IPuzzleService<SudokuPuzzle> _sudokuPuzzleService;

        public PuzzleHandler(WebSocketConnectionManager webSocketConnectionManager, IPuzzleService<SudokuPuzzle> sudokuPuzzleService) : base(webSocketConnectionManager)
        {
            _sudokuPuzzleService = sudokuPuzzleService;
        }

        public override Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            string json = Encoding.UTF8.GetString(buffer);
            PreMessage preMessage = JsonConvert.DeserializeObject<PreMessage>(json);
            switch (preMessage.EventName)
            {
                case "sudokuSolvePuzzleRequest":
                    return SudokuSolvePuzzleRequest(socket, json);
                case "sudokuGeneratePuzzleRequest":
                    return SudokuGeneratePuzzleRequest(socket);
                default:
                    return Task.CompletedTask;
            }
        }
        
        private async Task SudokuGeneratePuzzleRequest(WebSocket webSocket)
        {
            SudokuPuzzle puzzle = _sudokuPuzzleService.Generate();
            string puzzleStateAsString = puzzle.PuzzleState.ToString();
            PuzzleMessage<GeneratedSudokuPuzzle> generatedSudokuPuzzleMessage = new PuzzleMessage<GeneratedSudokuPuzzle>
            {
                Data = new GeneratedSudokuPuzzle
                {
                    PuzzleAsString = puzzleStateAsString
                },
                EventName = "sudokuGeneratePuzzleRequest"
            };

            await SendMessageAsync(webSocket, JsonConvert.SerializeObject(generatedSudokuPuzzleMessage));
        }

        private async Task SudokuSolvePuzzleRequest(WebSocket webSocket, string json)
        {
            PuzzleMessage<SudokuPuzzleState> sudokuPuzzleStateMessage = JsonConvert.DeserializeObject<PuzzleMessage<SudokuPuzzleState>>(json);
            SudokuPuzzle puzzle = new SudokuPuzzle(new PuzzleState(Encoding.Default.GetBytes(sudokuPuzzleStateMessage.Data.AsString)));
            PuzzleSolveDto solve = _sudokuPuzzleService.Solve(puzzle);
            string currentState = solve.PuzzleStates.First().ToString();
            foreach (PuzzleState state in solve.PuzzleStates.Skip(1).SkipLast(1))
            {
                SudokuStateChangeEvent eChangeEvent = GetSudokuStateChangeEvent(currentState, state.ToString());
                PuzzleMessage<SudokuStateChangeEvent> msg = new PuzzleMessage<SudokuStateChangeEvent>
                {
                    EventName = "sudokuPuzzleStateChange",
                    Data = eChangeEvent
                };
                currentState = state.ToString();
                // await Task.Delay(1);
                await SendMessageAsync(webSocket, JsonConvert.SerializeObject(msg));
            }

            PuzzleState puzzleState = solve.PuzzleStates.Last();
            SudokuStateChangeEvent changeEvent = GetSudokuStateChangeEvent(currentState, puzzleState.ToString());
            changeEvent.LastEvent = true;
            PuzzleMessage<SudokuStateChangeEvent> msg2 = new PuzzleMessage<SudokuStateChangeEvent>
            {
                EventName = "sudokuPuzzleStateChange",
                Data = changeEvent
            };
            // await Task.Delay(1);
            await SendMessageAsync(webSocket, JsonConvert.SerializeObject(msg2));
        }

        private static SudokuStateChangeEvent GetSudokuStateChangeEvent(string a, string b)
        {
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i])
                    return new SudokuStateChangeEvent { Changed = true, Index = i, NumberBefore = a[i] - 48, NumberAfter = b[i] - 48, LastEvent = false };
            }

            return new SudokuStateChangeEvent { Changed = true, Index = 80, NumberBefore = a[80], NumberAfter = b[80], LastEvent = false };
        }

        public class PreMessage
        {
            [JsonProperty("eventName")]
            public string EventName { get; set; }
        }
        public class PuzzleMessage<TMessageType>
        {
            [JsonProperty("eventName")]
            public string EventName { get; set; }
            [JsonProperty("data")]
            public TMessageType Data { get; set; }
        }
        public class SudokuStateChangeEvent
        {
            [JsonProperty("index")]
            public int Index { get; set; }
            [JsonProperty("numberAfter")]
            public int NumberAfter { get; set; }
            [JsonProperty("numberBefore")]
            public int NumberBefore { get; set; }
            [JsonProperty("changed")]
            public bool Changed { get; set; }
            [JsonProperty("lastEvent")]
            public bool LastEvent { get; set; }
            
        }

        public class GeneratedSudokuPuzzle
        {
            [JsonProperty("puzzleAsString")]
            public string PuzzleAsString { get; set; }
        }

        public class SudokuPuzzleState
        {
            [JsonProperty("asString")]
            public string AsString { get; set; }
            [JsonProperty("cells")]
            public Cell[] Cells { get; set; }
        }

        public class Data
        {
            [JsonProperty("puzzleState")]
            public Cell[] PuzzleState { get; set; }
            [JsonProperty("moves")]
            public object[] Moves { get; set; }
            [JsonProperty("puzzleLength")]
            public int PuzzleLength { get; set; }
        }

        public class Cell
        {
            [JsonProperty("row")]
            public int Row { get; set; }
            [JsonProperty("column")]
            public int Column { get; set; }
            [JsonProperty("number")]
            public int Number { get; set; }
            [JsonProperty("editable")]
            public bool Editable { get; set; }
        }
    }
}
