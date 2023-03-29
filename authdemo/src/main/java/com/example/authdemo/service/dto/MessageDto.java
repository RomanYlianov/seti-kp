package com.example.authdemo.service.dto;

import lombok.Data;

import javax.validation.constraints.Email;
import javax.validation.constraints.NotEmpty;
import javax.validation.constraints.NotNull;
import java.util.Date;

@Data
public class MessageDto {

    private Long id;

    private String senderUsername;

    private String receiverUsername;

    @NotEmpty(message = "messageText is mandatory")
    private String messageText;

    private Date creationTime;

    private Date modificationTime;
}
