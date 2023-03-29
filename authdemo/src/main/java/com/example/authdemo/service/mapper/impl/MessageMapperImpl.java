package com.example.authdemo.service.mapper.impl;

import com.example.authdemo.model.Account;
import com.example.authdemo.model.Message;
import com.example.authdemo.repository.AccountRepository;
import com.example.authdemo.service.dto.MessageDto;
import com.example.authdemo.service.mapper.MessageMapper;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.List;
import java.util.Optional;

@Service
public class MessageMapperImpl implements MessageMapper {

    Logger log = LoggerFactory.getLogger(MessageMapperImpl.class);

   private AccountRepository accountRepository;

   @Autowired
   public MessageMapperImpl(AccountRepository accountRepository)
   {
       this.accountRepository = accountRepository;
   }


    @Override
    public Message toEntity(MessageDto dto) {
        log.info("messageMapper: mapping dto to entity");
        Message entity = new Message();
        if (dto!=null)
        {
            if (dto.getId()!=null)
                entity.setId(dto.getId());
            if (dto.getSenderUsername()!=null)
            {
                Optional<Account> senderBox = accountRepository.findByEmail(dto.getSenderUsername());
                if (senderBox.isPresent())
                    entity.setSender(senderBox.get());
            }
            if (dto.getReceiverUsername()!=null)
            {
                Optional<Account> receiverBox = accountRepository.findByEmail(dto.getReceiverUsername());
                if (receiverBox.isPresent())
                    entity.setReceiver(receiverBox.get());
            }
            if (dto.getMessageText()!=null)
                entity.setMessageText(dto.getMessageText());
            if (dto.getCreationTime()!=null)
                entity.setCreationTime(dto.getCreationTime());
            if (dto.getModificationTime()!=null)
                entity.setModificationTime(dto.getModificationTime());
        }
        else
        {
            log.warn("messageMapper: input data is empty");
        }
        return entity;
    }

    @Override
    public List<Message> toEntity(List<MessageDto> dtoList) {
        log.info("messageMapper: mapping list of entities to list of dto");
        List<Message> entities = new ArrayList();
        if (dtoList!=null) {
            for (MessageDto dto : dtoList) {
                entities.add(toEntity(dto));
            }
        }
        else
        {
            log.warn("messageMapper: input data is empty");
        }
        return entities;
    }

    @Override
    public MessageDto toDto(Message entity) {
        log.info("messageMapper: mapping entity to dto");
        MessageDto dto = new MessageDto();
        if (entity!=null)
        {
            if (entity.getId()!=null)
                dto.setId(entity.getId());
            if (entity.getSender()!=null)
                dto.setSenderUsername(entity.getSender().getEmail());
            if (entity.getReceiver()!=null)
                dto.setReceiverUsername(entity.getReceiver().getEmail());
            if (entity.getMessageText()!=null)
                dto.setMessageText(entity.getMessageText());
            if (entity.getCreationTime()!=null)
                dto.setCreationTime(entity.getCreationTime());
            if (entity.getModificationTime()!=null)
                dto.setModificationTime(entity.getModificationTime());
        }
        else
        {
            log.warn("messageMapper: input data is empty");
        }
        return dto;
    }

    @Override
    public List<MessageDto> toDto(List<Message> entities) {
        log.info("messageMapper: mapping ist of entities to list of dto");
        List<MessageDto> dtoList = new ArrayList();
        if (entities!=null)
        {
            for (Message entity: entities)
                dtoList.add(toDto(entity));
        }
        else
        {
            log.warn("messageMapper: input data is empty");
        }
        return dtoList;
    }
}
