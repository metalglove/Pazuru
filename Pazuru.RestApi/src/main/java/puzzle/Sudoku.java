package puzzle;

import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;

@Entity
public class Sudoku {

	@Id
	@GeneratedValue(strategy = GenerationType.AUTO)
	private long id;
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
}
