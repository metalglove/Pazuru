using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pazuru.Application.DTOs;
using Pazuru.Application.Interfaces;
using Pazuru.Application.Services;
using Pazuru.Domain;
using Pazuru.Infrastructure.Services;
using Pazuru.Presentation.Web.BackEnd;
using Pazuru.Presentation.Web.BackEnd.Handlers;
using Pazuru.Presentation.Web.BackEnd.Models;
using Pazuru.Presentation.Web.BackEnd.Requests;
using Pazuru.Presentation.Web.BackEnd.Responses;
using Pazuru.Presentation.Web.BackEnd.Utilities;
using Pazuru.Sudoku;
using Pazuru.Tests.Mocks;

namespace Pazuru.Tests.Services
{
    [TestClass]
    public class PuzzleHandlerTests
    {
        private PuzzleHandler _puzzleHandler;
        private HttpClientHandlerMock _httpHandler;

        [TestInitialize]
        public void TestInitializer()
        {
            WebSocketConnectionManager webSocketConnectionManager = new WebSocketConnectionManager();
            PuzzleGenerator<SudokuPuzzle> sudokuGenerator = new SudokuGenerator();
            PuzzlePrinter<SudokuPuzzle> sudokuPrinter = new SudokuPrinter();
            IPuzzleService<SudokuPuzzle> sudokuPuzzleService = new SudokuPuzzleService(sudokuGenerator, sudokuPrinter);
            _httpHandler = new HttpClientHandlerMock();
            IRestServiceConnector restServiceConnector = new RestServiceConnector(_httpHandler);
            IPuzzleStorageService puzzleStorageService = new PuzzleStorageService(restServiceConnector);
            _puzzleHandler = new PuzzleHandler(webSocketConnectionManager, sudokuPuzzleService, puzzleStorageService);
        }

        [TestMethod]
        public async Task SudokuSolvePuzzleRequest_Should_Send_6598_PuzzleStateChanges()
        {
            SolveSudokuRequest solveSudokuRequest = new SolveSudokuRequest
            {
                Data = new SudokuPuzzleState
                {
                    PuzzleAsString = "034007008080065000000300070200000700710040096005000001050002000000170060600900430"
                },
                EventName = "sudokuSolvePuzzleRequest"
            };
            string json = JsonConvert.SerializeObject(solveSudokuRequest);
            byte[] messageBuffer = Encoding.Default.GetBytes(json);
            WebSocketMock webSocketMock = new WebSocketMock();
            _httpHandler.AddResponse(GetDefaultSavePuzzle());
            await _puzzleHandler.ReceiveAsync(webSocketMock, messageBuffer);

            Assert.IsTrue(webSocketMock.SentMessages.Count(tuple => tuple.preMessage.EventName.Equals("sudokuPuzzleStateChange")) == 6598);
        }
        [TestMethod]
        public async Task SudokuVerifyRequest_Should_Send_PuzzleState_With_27_CorrectIndexes()
        {
            VerifySudokuRequest sudokuVerifyRequest = new VerifySudokuRequest
            {
                Data = new VerifiableSudokuState
                {
                    PuzzleAsString = "034007008080065000000300070200000700710040096005000001050002000000170060600900430",
                    CurrentPuzzle = "034007008080065000000300070200000700710040096005000001050002000000170060600900430"
                },
                EventName = "sudokuVerifyRequest"
            };
            string json = JsonConvert.SerializeObject(sudokuVerifyRequest);
            byte[] messageBuffer = Encoding.Default.GetBytes(json);
            WebSocketMock webSocketMock = new WebSocketMock();
            await _puzzleHandler.ReceiveAsync(webSocketMock, messageBuffer);
            (_, ArraySegment<byte> arraySegment) = webSocketMock.SentMessages.Last();
            string responseJson = Encoding.Default.GetString(arraySegment);
            VerifySudokuPuzzleResponse verifySudokuPuzzleResponse = JsonConvert.DeserializeObject<VerifySudokuPuzzleResponse>(responseJson);
            Assert.IsTrue(verifySudokuPuzzleResponse.Data.CorrectIndexes.Length == 27);
        }
        
        [TestMethod]
        public async Task SudokuGeneratePuzzleRequest_Should_Send_A_Valid_SudokuPuzzleState()
        {
            PreMessage sudokuGeneratePuzzleRequest = new PreMessage
            {
                EventName = "sudokuGeneratePuzzleRequest"
            };
            string json = JsonConvert.SerializeObject(sudokuGeneratePuzzleRequest);
            byte[] messageBuffer = Encoding.Default.GetBytes(json);
            WebSocketMock webSocketMock = new WebSocketMock();
            await _puzzleHandler.ReceiveAsync(webSocketMock, messageBuffer);
            (_, ArraySegment<byte> arraySegment) = webSocketMock.SentMessages.First();
            string responseJson = Encoding.Default.GetString(arraySegment);
            SudokuGeneratePuzzleResponse sudokuGeneratePuzzleResponse = JsonConvert.DeserializeObject<SudokuGeneratePuzzleResponse>(responseJson);

            Assert.IsTrue(sudokuGeneratePuzzleResponse.Success);
        }
        [TestMethod]
        public async Task PreviouslySolvedPuzzlesRequest_Should_Send_An_Empty_List()
        {
            PreMessage previouslySolvedPuzzlesRequest = new PreMessage
            {
                EventName = "previouslySolvedPuzzlesRequest"
            };
            string json = JsonConvert.SerializeObject(previouslySolvedPuzzlesRequest);
            byte[] messageBuffer = Encoding.Default.GetBytes(json);
            WebSocketMock webSocketMock = new WebSocketMock();

            _httpHandler.AddResponse(GetPreviouslySolvedPuzzles(new List<PuzzleDto>()));

            await _puzzleHandler.ReceiveAsync(webSocketMock, messageBuffer);
            (_, ArraySegment<byte> arraySegment) = webSocketMock.SentMessages.First();
            string responseJson = Encoding.Default.GetString(arraySegment);
            PreviouslySolvedPuzzlesResponse sudokuGeneratePuzzleResponse = JsonConvert.DeserializeObject<PreviouslySolvedPuzzlesResponse>(responseJson);

            Assert.IsTrue(sudokuGeneratePuzzleResponse.Success);
            Assert.IsFalse(sudokuGeneratePuzzleResponse.Data.Puzzles.Any());
        }
        [TestMethod]
        public async Task PreviouslySolvedPuzzlesRequest_Should_Send_A_List_With_One_SudokuPuzzle()
        {
            SolveSudokuRequest solveSudokuRequest = new SolveSudokuRequest
            {
                Data = new SudokuPuzzleState
                {
                    PuzzleAsString = "034007008080065000000300070200000700710040096005000001050002000000170060600900430"
                },
                EventName = "sudokuSolvePuzzleRequest"
            };
            string jsonSolve = JsonConvert.SerializeObject(solveSudokuRequest);
            byte[] messageBufferSolve = Encoding.Default.GetBytes(jsonSolve);
            WebSocketMock webSocketMockSolver = new WebSocketMock();
            _httpHandler.AddResponse(GetDefaultSavePuzzle());
            await _puzzleHandler.ReceiveAsync(webSocketMockSolver, messageBufferSolve);

            PreMessage previouslySolvedPuzzlesRequest = new PreMessage
            {
                EventName = "previouslySolvedPuzzlesRequest"
            };
            string json = JsonConvert.SerializeObject(previouslySolvedPuzzlesRequest);
            byte[] messageBuffer = Encoding.Default.GetBytes(json);
            WebSocketMock webSocketMock = new WebSocketMock();
            PuzzleDto puzzleDto = new PuzzleDto
            {
                OriginalPuzzle = "034007008080065000000300070200000700710040096005000001050002000000170060600900430",
                PuzzleId = 1,
                PuzzleType = "Sudoku",
                SolvedPuzzle = "534297618187465329962381574246819753718543296395726841459632187823174965671958432"
            };

            _httpHandler.AddResponse(GetPreviouslySolvedPuzzles(new List<PuzzleDto> { puzzleDto }));

            await _puzzleHandler.ReceiveAsync(webSocketMock, messageBuffer);
            (_, ArraySegment<byte> arraySegment) = webSocketMock.SentMessages.First();
            string responseJson = Encoding.Default.GetString(arraySegment);
            PreviouslySolvedPuzzlesResponse sudokuGeneratePuzzleResponse = JsonConvert.DeserializeObject<PreviouslySolvedPuzzlesResponse>(responseJson);

            Assert.IsTrue(sudokuGeneratePuzzleResponse.Success);
            Assert.IsTrue(sudokuGeneratePuzzleResponse.Data.Puzzles.Count(puzzle => puzzle.PuzzleType.Equals("Sudoku")) == 1);
        }

        private static string GetPreviouslySolvedPuzzles(IEnumerable<PuzzleDto> puzzles)
        {
            return JsonConvert.SerializeObject(new PuzzleStorageService.HalRootObject<PuzzleStorageService.PuzzleDtoHal[]>
            {
                Data = puzzles.Select(puzzle => new PuzzleStorageService.PuzzleDtoHal()
                {
                    SolvedPuzzle = puzzle.SolvedPuzzle,
                    OriginalPuzzle = puzzle.OriginalPuzzle,
                    PuzzleId = puzzle.PuzzleId,
                    PuzzleType = puzzle.PuzzleType,
                    Links = new PuzzleStorageService.Links()
                    {
                        Self = new PuzzleStorageService.Self()
                        {
                            Href = "http://localhost:8090/puzzles/" + puzzle.PuzzleType.ToLower() + "/{id}",
                            Templated = true
                        }
                    }
                }).ToArray(),
                Links = new PuzzleStorageService.Links()
                {
                    Self = new PuzzleStorageService.Self()
                    {
                        Href = "http://localhost:8090/puzzles/previouslySolvedPuzzles",
                        Templated = false
                    }
                },
                Message = "Successfully found all previously solved puzzles.",
                Success = true
            });
        }
        private static string GetDefaultSavePuzzle()
        {
            return
                new JObject(
                        new JProperty("message", "Saved successfully."),
                        new JProperty("success", true),
                        new JProperty("data",
                            new JObject(new JProperty("puzzleId", 1), new JProperty("puzzleType", "Sudoku"),
                                new JProperty("solvedPuzzle",
                                    "534297618187465329962381574246819753718543296395726841459632187823174965671958432"),
                                new JProperty("originalPuzzle",
                                    "034007008080065000000300070200000700710040096005000001050002000000170060600900430"),
                                new JProperty("_links",
                                    new JObject(new JProperty("self",
                                        new JObject(new JProperty("href", "http://localhost:8090/puzzles/sudoku/{id}"),
                                            new JProperty("templated", true))))))),
                        new JProperty("_links",
                            new JObject(new JProperty("self",
                                new JObject(new JProperty("href", "http://localhost:8090/puzzles/savePuzzle"))))))
                    .ToString();
        }
    }
}
