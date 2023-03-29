package com.example.authdemo.service;

import com.example.authdemo.config.InvalidRequestException;
import com.example.authdemo.service.dto.MessageDto;
import com.example.authdemo.service.dto.MessageSearchDto;
import org.springframework.data.domain.Pageable;
import org.springframework.stereotype.Service;
import java.util.List;
import java.util.Optional;

@Service
public interface MessageService {

    List<MessageDto> searchMessages(MessageSearchDto dto, String username, Pageable pageable) throws InvalidRequestException;

    Optional<MessageDto> getMessageById(Long id, String username) throws InvalidRequestException;

    Optional<MessageDto> sendMessage(MessageDto dto, String username) throws InvalidRequestException;

    Optional<MessageDto> updateMessage(Long id, MessageDto dto, String username) throws InvalidRequestException;

    void removeMessage(Long id, String username) throws InvalidRequestException;

    void removeMessages(MessageSearchDto dto, String username, Pageable pageable) throws InvalidRequestException;

}
