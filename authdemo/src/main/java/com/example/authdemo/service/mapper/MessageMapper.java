package com.example.authdemo.service.mapper;

import com.example.authdemo.model.Message;
import com.example.authdemo.service.dto.MessageDto;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public interface MessageMapper {

    Message toEntity(MessageDto dto);

    List<Message> toEntity(List<MessageDto> dtoList);

    MessageDto toDto(Message entity);

    List<MessageDto> toDto(List<Message> entities);

}
