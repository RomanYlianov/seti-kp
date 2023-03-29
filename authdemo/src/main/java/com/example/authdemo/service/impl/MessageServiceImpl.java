package com.example.authdemo.service.impl;

import com.example.authdemo.config.InvalidRequestException;
import com.example.authdemo.model.Account;
import com.example.authdemo.model.Message;
import com.example.authdemo.model.Role;
import com.example.authdemo.repository.AccountRepository;
import com.example.authdemo.repository.MessageRepository;
import com.example.authdemo.service.MessageService;
import com.example.authdemo.service.dto.MessageDto;
import com.example.authdemo.service.dto.MessageSearchDto;
import com.example.authdemo.service.mapper.AccountMapper;
import com.example.authdemo.service.mapper.MessageMapper;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.web.bind.annotation.RequestBody;

import javax.validation.Valid;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.Optional;

@Service
public class MessageServiceImpl implements MessageService {

    private Logger log = LoggerFactory.getLogger(MessageServiceImpl.class);

    private AccountRepository accountRepository;

    private MessageRepository messageRepository;

    private AccountMapper accountMapper;

    private MessageMapper messageMapper;

    @Autowired
    public MessageServiceImpl(AccountRepository accountRepository, MessageRepository messageRepository, AccountMapper accountMapper, MessageMapper messageMapper) {
        this.accountRepository = accountRepository;
        this.messageRepository = messageRepository;
        this.accountMapper = accountMapper;
        this.messageMapper = messageMapper;
    }


    @Transactional
    @Override
    public List<MessageDto> searchMessages(MessageSearchDto dto, String username, Pageable pageable) throws InvalidRequestException {
        log.info("search message by parameters");
        if (dto!=null && username!=null)
        {
            Boolean isAdmin = isAdminUser(username);
            dto.setSenderUsername(username);
            List<MessageDto> list = new ArrayList();
            SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd hh:mm:ss");
            Date creationTimeStart = null;
            Date creationTimeEnd = null;
            Date modificationTimeStart = null;
            Date modificationTimeEnd = null;
            if (dto.getCreationTimeStart()!=null)
            {
                try {
                    creationTimeStart = sdf.parse(dto.getCreationTimeStart());
                }
                catch (ParseException ex)
                {
                    String message = "creationTimeStart syntax is invalid";
                    log.warn(message);
                    throw new InvalidRequestException(HttpStatus.BAD_REQUEST, message);
                }
            }
            if (dto.getCreationTimeEnd()!=null)
            {
                try {
                    creationTimeEnd = sdf.parse(dto.getCreationTimeEnd());
                }
                catch (ParseException ex)
                {
                    String message = "creationTimeEnd syntax is invalid";
                    log.warn(message);
                    throw new InvalidRequestException(HttpStatus.BAD_REQUEST, message);
                }
            }
            if (dto.getModificationTimeStart()!=null)
            {
                try
                {
                    modificationTimeStart = sdf.parse(dto.getModificationTimeStart());
                }
                catch (ParseException ex)
                {
                    String message = "modificationTimeStart syntax is invalid";
                    log.warn(message);
                    throw new InvalidRequestException(HttpStatus.BAD_REQUEST, message);
                }
            }
            if (dto.getModificationTimeEnd()!=null)
            {
                try
                {
                    modificationTimeEnd = sdf.parse(dto.getModificationTimeEnd());
                }
                catch (ParseException ex)
                {
                    String message = "modificationTimeEnd syntax is invalid";
                    log.warn(message);
                    throw new InvalidRequestException(HttpStatus.BAD_REQUEST, message);
                }
            }
            log.info("sender "+dto.getSenderUsername());
            log.info("receiver "+dto.getReceiverUsername());
            log.info("message text "+dto.getMessageText());



            Page<Message> page = messageRepository.findBySender_emailContainingAndReceiver_emailContainingAndMessageTextContaining(dto.getSenderUsername(),dto.getReceiverUsername(), dto.getMessageText(), pageable);
            if (!page.isEmpty())
            {
                for(Message entity: page.toList())
                {
                    Boolean flag = true;
                    if (creationTimeStart!=null)
                    {
                        flag =  creationTimeStart.getTime()>=entity.getCreationTime().getTime();
                    }
                    if (creationTimeEnd!=null)
                    {
                        if (flag)
                        {
                            flag = creationTimeEnd.getTime()>=entity.getCreationTime().getTime();
                        }
                    }
                    if (modificationTimeStart!=null)
                    {
                        if (flag)
                        {
                            if (entity.getModificationTime()!=null)
                            {
                                flag = modificationTimeStart.getTime()<=modificationTimeEnd.getTime();
                            }
                        }
                    }
                    if (modificationTimeEnd!=null)
                    {
                        if (flag)
                        {
                            if (entity.getModificationTime()!=null)
                            {
                                flag = modificationTimeStart.getTime()<=modificationTimeEnd.getTime();
                            }
                        }
                    }
                    if (flag)
                    {
                        if (!isAdmin)
                        {
                            if (entity.getSender().getEmail().equals(username) || entity.getReceiver().getEmail().equals(username))
                            {
                                list.add(messageMapper.toDto(entity));
                            }
                        }
                        else
                        {
                            list.add(messageMapper.toDto(entity));
                        }
                    }
                }
            }
            if (list.isEmpty())
            {
                String message = "message not found, please check search parameters and try again";
                log.warn(message);
                throw new InvalidRequestException(HttpStatus.NOT_FOUND, message);
            }
            return list;
        }
        else
        {
            String message = "input data is empty";
            log.warn(message);
            throw new InvalidRequestException(HttpStatus.BAD_REQUEST, message);
        }
    }


    @Transactional
    @Override
    public Optional<MessageDto> getMessageById(Long id, String username) throws InvalidRequestException {
        if (id!=null && username!=null)
        {
            if (id>0)
            {
                Boolean isAdmin = isAdminUser(username);
                Optional<MessageDto> result = Optional.empty();
                Optional<Message> box = messageRepository.findById(id);
                if (box.isPresent())
                {
                    Message entity = box.get();
                    if (isAdmin)
                    {
                        result = Optional.ofNullable(messageMapper.toDto(entity));
                    }
                    else
                    {
                        if (entity.getSender().getEmail().equals(username) || entity.getReceiver().getEmail().equals(username))
                        {
                            result = Optional.ofNullable(messageMapper.toDto(entity));
                        }
                        else
                        {
                            String message = "trying to get not yourself message";
                            log.warn(message);
                            throw new InvalidRequestException(HttpStatus.FORBIDDEN, message);
                        }
                    }
                }
                if (result.isPresent())
                {
                    return result;
                }
                else
                {
                    String message = "message not found, please check search parameters and try again";
                    log.warn(message);
                    throw new InvalidRequestException(HttpStatus.NOT_FOUND, message);
                }
            }
            else
            {
                String message = "id must be positive";
                log.warn(message);
                throw new InvalidRequestException(HttpStatus.BAD_REQUEST, message);
            }
        }
        else
        {
            String message = "input data is empty";
            log.warn(message);
            throw new InvalidRequestException(HttpStatus.BAD_REQUEST, message);
        }
    }

    @Transactional
    @Override
    public Optional<MessageDto> sendMessage( MessageDto dto, String username) throws InvalidRequestException {
        log.info("sending message");
        if (dto!=null)
        {
            if (dto.getReceiverUsername()!=null)
            {
                Message entity = messageMapper.toEntity(dto);
                Optional<Account> senderBox = accountRepository.findByEmail(username);
                Optional<Account> receiverBox = accountRepository.findByEmail(dto.getReceiverUsername());
                if (receiverBox.isPresent())
                {
                    if (senderBox.isPresent())
                        entity.setSender(senderBox.get());
                    entity.setReceiver(receiverBox.get());
                    entity.setCreationTime(new Date());
                    entity = messageRepository.save(entity);
                    return Optional.ofNullable(messageMapper.toDto(entity));
                }
                else
                {
                    String message = "receiver with email "+dto.getReceiverUsername()+" was not found";
                    log.warn(message);
                    throw new InvalidRequestException(HttpStatus.NOT_FOUND, message);
                }
            }
            else
            {
                String message = "receiver field is mandatory";
                log.warn(message);
                throw new InvalidRequestException(HttpStatus.BAD_REQUEST, message);
            }
        }
        else
        {
            String message = "input data is empty";
            log.warn(message);
            throw new InvalidRequestException(HttpStatus.BAD_REQUEST, message);
        }
    }



    @Transactional
    @Override
    public Optional<MessageDto> updateMessage(Long id, MessageDto dto, String username) throws InvalidRequestException {
        log.info("updating message");
        if (id!=null && username!=null)
        {
            if (id>0)
            {
                Optional<Message> box = messageRepository.findById(id);
                Boolean isAdmin = isAdminUser(username);
                if (box.isPresent())
                {
                    Message entity = box.get();
                    if (!isAdmin && !entity.getSender().getEmail().equals(username))
                    {
                        String message = "trying to update not yourself message";
                        log.warn(message);
                        throw new InvalidRequestException(HttpStatus.FORBIDDEN, message);
                    }
                    entity.setMessageText(dto.getMessageText());
                    entity.setModificationTime(new Date());
                    entity = messageRepository.save(entity);
                    return Optional.ofNullable(messageMapper.toDto(entity));
                }
                else
                {
                    String message = "message with id "+id+" was not found";
                    log.warn(message);
                    throw new InvalidRequestException(HttpStatus.NOT_FOUND, message);
                }
            }
            else
            {
                String message = "id must be positive";
                log.warn(message);
                throw new InvalidRequestException(HttpStatus.BAD_REQUEST, message);
            }
        }
        else
        {
            String message = "input data is empty";
            log.warn(message);
            throw new InvalidRequestException(HttpStatus.BAD_REQUEST, message);
        }
    }

    @Transactional
    @Override
    public void removeMessage(Long id, String username) throws InvalidRequestException {
        log.info("remove message");
        if (id!=null && username!=null)
        {
            if (id>0)
            {
                Optional<Message> box = messageRepository.findById(id);
                if (box.isPresent())
                {
                    Message entity = box.get();
                    Boolean isAdmin = isAdminUser(username);
                    if (!isAdmin && !entity.getSender().getEmail().equals(username))
                    {
                        String message = "trying to remove not yourself message";
                        log.warn(message);
                        throw new InvalidRequestException(HttpStatus.FORBIDDEN, message);
                    }
                    messageRepository.deleteById(id);
                }
                else
                {
                    String message = "message with id "+id+"was not found";
                    log.warn(message);
                    throw new InvalidRequestException(HttpStatus.NOT_FOUND, message);
                }
            }
            else
            {
                String message = "id must be positive";
                log.warn(message);
                throw new InvalidRequestException(HttpStatus.BAD_REQUEST, message);
            }
        }
        else
        {
            String message = "input data is empty";
            log.warn(message);
            throw new InvalidRequestException(HttpStatus.BAD_REQUEST, message);
        }
    }

    @Transactional
    @Override
    public void removeMessages(MessageSearchDto dto, String username, Pageable pageable) throws InvalidRequestException {
        log.info("delete messages by parameter");
        if (dto!=null && username!=null)
        {
            Page<Message> page = messageRepository.findBySender_emailContainingAndReceiver_emailContainingAndMessageTextContaining(dto.getSenderUsername(),dto.getReceiverUsername(), dto.getMessageText(), pageable);
            Boolean isAdmin = isAdminUser(username);
            if (page.isEmpty())
            {
                String message = "message not found, please check search parameters and try again";
                log.warn(message);
                throw new InvalidRequestException(HttpStatus.NOT_FOUND, message);
            }

            Date creationTimeStart = null;
            Date creationTimeEnd = null;
            Date modificationTimeStart = null;
            Date modificationTimeEnd = null;
            SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd hh:mm:ss");
            if (dto.getCreationTimeStart()!=null)
            {
                 try
                 {
                    creationTimeStart = sdf.parse(dto.getCreationTimeStart());
                 }
                 catch (ParseException ex)
                 {
                     String message = "creationTimeStart syntax is invalid";
                     log.warn(message);
                     throw new InvalidRequestException(HttpStatus.BAD_REQUEST, message);
                 }
            }
            if (dto.getCreationTimeEnd()!=null)
            {
                try{
                    creationTimeEnd = sdf.parse(dto.getCreationTimeEnd());
                }
                catch (ParseException ex)
                {
                    String message = "creationTimeEnd syntax is invalid";
                    log.warn(message);
                    throw new InvalidRequestException(HttpStatus.BAD_REQUEST, message);
                }
            }
            if (dto.getModificationTimeStart()!=null)
            {
                try{
                    modificationTimeStart = sdf.parse(dto.getModificationTimeStart());
                }
                catch (ParseException ex)
                {
                    String message = "modificationTimeStart syntax is invalid";
                    log.warn(message);
                    throw new InvalidRequestException(HttpStatus.BAD_REQUEST, message);
                }
            }
            if (dto.getModificationTimeEnd()!=null)
            {
                try{
                    modificationTimeEnd = sdf.parse(dto.getModificationTimeEnd());
                }
                catch (ParseException ex)
                {
                    String message = "modificationTimeEnd syntax is invalid";
                    log.warn(message);
                    throw new InvalidRequestException(HttpStatus.BAD_REQUEST, message);
                }
            }
            for (Message entity: page.toList())
            {
                Boolean flag = true;
                if (creationTimeStart!=null)
                {
                    flag =  creationTimeStart.getTime()>=entity.getCreationTime().getTime();
                }
                if (creationTimeEnd!=null)
                {
                    if (flag)
                    {
                        flag = creationTimeEnd.getTime()>=entity.getCreationTime().getTime();
                    }
                }
                if (modificationTimeStart!=null)
                {
                    if (flag)
                    {
                        if (entity.getModificationTime()!=null)
                        {
                            flag = modificationTimeStart.getTime()<=modificationTimeEnd.getTime();
                        }
                    }
                }
                if (modificationTimeEnd!=null)
                {
                    if (flag)
                    {
                        if (entity.getModificationTime()!=null)
                        {
                            flag = modificationTimeStart.getTime()<=modificationTimeEnd.getTime();
                        }
                    }
                }
                if (flag)
                {
                    if (!isAdmin && !entity.getSender().getEmail().equals(username))
                    {
                        String message = "trying to remove not yourself message";
                        log.warn(message);
                        throw new InvalidRequestException(HttpStatus.FORBIDDEN, message);
                    }
                }
            }
            messageRepository.deleteAll(page.toList());
        }
        else
        {
            String message = "input data is empty";
            log.warn(message);
            throw new InvalidRequestException(HttpStatus.BAD_REQUEST, message);
        }
    }

    @Transactional
    Boolean isAdminUser(String username)
    {
        log.info("check user permissions");
        Boolean isAdmin = false;
        if (username!=null)
        {
            Optional<Account> box = accountRepository.findByEmail(username);
            if (box.isPresent())
            {
                isAdmin = box.get().getRoles().contains(Role.ADMIN);
            }
        }
       return isAdmin;
    }
}
