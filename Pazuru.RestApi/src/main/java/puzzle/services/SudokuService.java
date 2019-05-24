package puzzle.services;

import java.util.List;
import java.util.Optional;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import puzzle.entities.Sudoku;
import puzzle.repositories.SudokuRepository;
import com.google.common.collect.Lists;

@Service
public class SudokuService {

    @Autowired
    private SudokuRepository sudokuRepository;

    public List<Sudoku> getAll() {
        return Lists.newArrayList(sudokuRepository.findAll());
	}

	public Optional<Sudoku> find(String sudokuId) {
		return sudokuRepository.findById(Long.parseLong(sudokuId));
	}

	public Sudoku save(Sudoku puzzle) {
		return sudokuRepository.save(puzzle);
	}

}