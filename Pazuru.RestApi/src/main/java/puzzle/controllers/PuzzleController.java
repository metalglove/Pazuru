package puzzle.controllers;

import java.util.ArrayList;
import java.util.List;
import java.util.Optional;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.hateoas.Link;
import org.springframework.hateoas.Resources;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import static org.springframework.hateoas.mvc.ControllerLinkBuilder.linkTo;
import static org.springframework.hateoas.mvc.ControllerLinkBuilder.methodOn;

import puzzle.entities.Hitori;
import puzzle.entities.Sudoku;
import puzzle.services.HitoriService;
import puzzle.services.SudokuService;

@RestController
@RequestMapping(value = "/puzzles")
public class PuzzleController {
    
    @Autowired
    private SudokuService sudokuService;

    @Autowired
    private HitoriService hitoriService;

    @GetMapping(value = "/previouslySolvedPuzzles", produces = { "application/hal+json" })
    public ResultDto<List<Puzzle>> getPreviouslySolvedPuzzles() {
        List<Puzzle> puzzles = new ArrayList<>();
        List<Sudoku> sudokus = sudokuService.getAll();
        List<Hitori> hitoris = hitoriService.getAll();
        for (final Sudoku sudoku : sudokus) {
            String sudokuId = String.valueOf(sudoku.getSudokuId());
            Link selfLink = linkTo(methodOn(PuzzleController.class).getSudokuById(sudokuId)).withSelfRel();
            Puzzle puzzle = new Puzzle();
            puzzle.setPuzzleId(sudoku.getSudokuId());
            puzzle.setPuzzleType(sudoku.getClass().getSimpleName());
            puzzle.setSolvedPuzzle(sudoku.getSolvedSudoku());
            puzzle.setOriginalPuzzle(sudoku.getOriginalSudoku());
            puzzle.add(selfLink);
            puzzles.add(puzzle);
        }
        for (final Hitori hitori : hitoris) {
            String hitoriId = String.valueOf(hitori.getHitoriId());
            Link selfLink = linkTo(methodOn(PuzzleController.class).getHitoriById(hitoriId)).withSelfRel();
            Puzzle puzzle = new Puzzle();
            puzzle.setPuzzleId(hitori.getHitoriId());
            puzzle.setPuzzleType(hitori.getClass().getSimpleName());
            puzzle.setSolvedPuzzle(hitori.getSolvedHitori());
            puzzle.setOriginalPuzzle(hitori.getOriginalHitori());
            puzzle.add(selfLink);
            puzzles.add(puzzle);
        }
        Link link = linkTo(methodOn(PuzzleController.class).getPreviouslySolvedPuzzles()).withSelfRel();
        ResultDto<List<Puzzle>> resultDto = new ResultDto<List<Puzzle>>();
        resultDto.setData(puzzles);
        resultDto.setSuccess(true);
        resultDto.setMessage("Successfully found all previously solved puzzles.");
        resultDto.add(link);
        return resultDto;
    }

    @GetMapping(value = "/sudoku/{id}", produces = { "application/json" })
    public Sudoku getSudokuById(@PathVariable final String sudokuId) {
        Optional<Sudoku> sudoku = sudokuService.find(sudokuId);
        if (sudoku.isPresent())
            return sudoku.get();
        else
            return null;
    }

    @GetMapping(value = "/hitori/{id}", produces = { "application/json" })
    public Hitori getHitoriById(@PathVariable final String hitoriId) {
        Optional<Hitori> hitori = hitoriService.find(hitoriId);
        if (hitori.isPresent())
            return hitori.get();
        else
            return null;
    }

    @PostMapping(value = "/savePuzzle", produces = { "application/hal+json" })
    public ResultDto<Puzzle> postPuzzle(@RequestBody final Puzzle puzzleBody) {
        ResultDto<Puzzle> resultDto;

        if ("Sudoku".equals(puzzleBody.getPuzzleType())) {
            Sudoku sudokuToSave = new Sudoku();
            sudokuToSave.setOriginalSudoku(puzzleBody.getOriginalPuzzle());
            sudokuToSave.setSolvedSudoku(puzzleBody.getSolvedPuzzle());
            Sudoku savedSudoku = sudokuService.save(sudokuToSave);
            puzzleBody.setPuzzleId(savedSudoku.getSudokuId());
            String sudokuId = String.valueOf(savedSudoku.getSudokuId());
            Link selfLink = linkTo(methodOn(PuzzleController.class).getSudokuById(sudokuId)).withSelfRel();
            puzzleBody.add(selfLink);
            resultDto = new ResultDto<Puzzle>();
            resultDto.setData(puzzleBody);
            resultDto.setSuccess(true);
            resultDto.setMessage("Saved successfully.");
        } else if ("Hitori".equals(puzzleBody.getPuzzleType())) {
            Hitori hitoriToSave = new Hitori();
            hitoriToSave.setOriginalHitori(puzzleBody.getOriginalPuzzle());
            hitoriToSave.setSolvedHitori(puzzleBody.getSolvedPuzzle());
            Hitori savedHitori = hitoriService.save(hitoriToSave);
            puzzleBody.setPuzzleId(savedHitori.getHitoriId());
            String hitoriId = String.valueOf(savedHitori.getHitoriId());
            Link selfLink = linkTo(methodOn(PuzzleController.class).getSudokuById(hitoriId)).withSelfRel();
            puzzleBody.add(selfLink);
            resultDto = new ResultDto<Puzzle>();
            resultDto.setData(puzzleBody);
            resultDto.setSuccess(true);
            resultDto.setMessage("Saved successfully.");
        }
        else {
            resultDto = new ResultDto<>();
            resultDto.setSuccess(false);
            resultDto.setMessage("Failed to save puzzle.");
        }
        Link link = linkTo(methodOn(PuzzleController.class).postPuzzle(puzzleBody)).withSelfRel();
        resultDto.add(link);
        return resultDto;
    }
}
