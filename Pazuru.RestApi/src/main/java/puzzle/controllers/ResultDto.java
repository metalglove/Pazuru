package puzzle.controllers;

import org.springframework.hateoas.ResourceSupport;

public class ResultDto<T> extends ResourceSupport {
    private String message;
    private boolean success;
    private T data;

    public boolean isSuccess() {
        return success;
    }

    public T getData() {
        return data;
    }

    public void setData(T data) {
        this.data = data;
    }

    public String getMessage() {
        return message;
    }

    public void setMessage(String message) {
        this.message = message;
    }

    public void setSuccess(boolean success) {
        this.success = success;
    }
}