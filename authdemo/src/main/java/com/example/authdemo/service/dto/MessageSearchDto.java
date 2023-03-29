package com.example.authdemo.service.dto;

import lombok.Data;
import javax.validation.constraints.NotEmpty;
import javax.validation.constraints.NotNull;
import java.util.Date;

@Data
public class MessageSearchDto {

    @NotEmpty(message = "senderUserName is mandatory")
    private String senderUsername;

    @NotEmpty(message = "receiverUsername is mandatory")
    private String receiverUsername;

    @NotEmpty(message = "messageText is mandatory")
    private String messageText;

    private String creationTimeStart;

    private String creationTimeEnd;

    private String modificationTimeStart;

    private String modificationTimeEnd;
}
