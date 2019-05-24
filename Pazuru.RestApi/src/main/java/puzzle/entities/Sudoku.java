package puzzle.entities;

import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;

import org.springframework.hateoas.ResourceSupport;

@Entity
public class Sudoku extends ResourceSupport {

	@Id
	@GeneratedValue(strategy = GenerationType.AUTO)
	private Long id;
	private String solvedSudoku;
	private String originalSudoku;

	public String getSolvedSudoku() {
		return solvedSudoku;
	}

	public String getOriginalSudoku() {
		return originalSudoku;
	}

	public void setOriginalSudoku(String originalSudoku) {
		this.originalSudoku = originalSudoku;
	}

	public void setSolvedSudoku(String solvedSudoku) {
		this.solvedSudoku = solvedSudoku;
	}

	public Long getSudokuId() {
		return id;
	}

	public void setSudokuId(Long id) {
		this.id = id;
	}
}
