package puzzle.controllers;

import org.springframework.hateoas.ResourceSupport;

public class Puzzle extends ResourceSupport {
    private Long puzzleId;
    private String puzzleType;
    private String solvedPuzzle;
    private String originalPuzzle;

    public Long getPuzzleId() {
        return puzzleId;
    }

    public String getOriginalPuzzle() {
        return originalPuzzle;
    }

    public void setOriginalPuzzle(String originalPuzzle) {
        this.originalPuzzle = originalPuzzle;
    }

    public String getSolvedPuzzle() {
        return solvedPuzzle;
    }

    public void setSolvedPuzzle(String solvedPuzzle) {
        this.solvedPuzzle = solvedPuzzle;
    }

    public String getPuzzleType() {
        return puzzleType;
    }

    public void setPuzzleType(String puzzleType) {
        this.puzzleType = puzzleType;
    }

    public void setPuzzleId(Long puzzleId) {
        this.puzzleId = puzzleId;
    }
}