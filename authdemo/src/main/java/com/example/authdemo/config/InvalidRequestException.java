package com.example.authdemo.config;

import lombok.Getter;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.http.HttpStatus;

@Getter
public class InvalidRequestException extends Exception{

    private Logger log = LoggerFactory.getLogger(InvalidRequestException.class);

    private HttpStatus statusCode;

    public InvalidRequestException(HttpStatus statusCode, String message)
    {
        super(message);
        log.info("initialize InvalidRequestException: message: {}, http status code {}",message,statusCode.name());
        this.statusCode = statusCode;
    }
}
