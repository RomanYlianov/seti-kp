package com.example.authdemo.controller;

import com.example.authdemo.config.InvalidRequestException;
import com.example.authdemo.service.MessageService;
import com.example.authdemo.service.dto.*;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Pageable;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.*;

import javax.validation.Valid;
import java.security.Principal;
import java.util.List;
import java.util.Optional;

@RestController
@RequestMapping("/messages")
public class MessageRestController {

    private Logger log = LoggerFactory.getLogger(MessageRestController.class);

    private MessageService messageService;

    @Autowired
    public MessageRestController(MessageService messageService) {
        this.messageService = messageService;
    }

    /**
     * поиск сообшений
     * может быть выполнена как обычным пользователем, так и администратором
     * @param dto
     * @return
     * 200 - найдена информация о сообшениях
     * 404 - не найдено ни одного сообщения
     * 401 - анонимный запрос
     */
    @GetMapping("/search")
    @PreAuthorize("hasAnyAuthority('ADMIN', 'USER')")
    public ResponseEntity<List<MessageDto>> searchMessages(@Valid MessageSearchDto dto, Principal principal, Pageable pageable)
    {
        log.info("searching messages by parameters");
        try
        {
            List<MessageDto> list = messageService.searchMessages(dto, principal.getName(), pageable);
            return new ResponseEntity(list, HttpStatus.OK);
        }
        catch (InvalidRequestException ex)
        {
            return new ResponseEntity(ex.getStatusCode());
        }
    }

    /**
     * получение информаци о сообщении по id
     * может быть выполнен как пользователем, так и администратором
     * @param id
     * @param principal
     * @return
     * 200 - получена информаци о сообщении
     * 404 - сообщение не найдено
     * 403 - авторизованный пользователь неадминистратор и его нет в списке отправителя или получателя сообщения
     * 401 - анонимный запрос
     * 400 - неверные входные параметры
     */
    @GetMapping("/{id}")
    @PreAuthorize("hasAnyAuthority('USER','ADMIN')")
    public ResponseEntity<MessageDto> getMessageById(@PathVariable Long id, Principal principal)
    {
        log.info("get message by id");
        try{
            Optional<MessageDto> box = messageService.getMessageById(id, principal.getName());
            return new ResponseEntity<>(box.get(), HttpStatus.OK);
        }
        catch (InvalidRequestException ex)
        {
            return new ResponseEntity<>(ex.getStatusCode());
        }
    }

    /**
     * посылка сообщения
     * может быть выполнена как пользователем, так и администратором
     * @param dto
     * @param principal
     * @return
     * 201 - успешная отпрравка
     * 204 - сообщение не отправлено
     * 401 - анонимный запрос
     * 400 - неверные входные параеметры
     */
    @PostMapping
    @PreAuthorize("hasAnyAuthority('USER','ADMIN')")
    public ResponseEntity<MessageDto> sendMessage(@Valid @RequestBody MessageDto dto, Principal principal)
    {
        log.info("sending message");
        try
        {
            Optional<MessageDto> box = messageService.sendMessage(dto, principal.getName());
            if (box.isPresent())
            {
                return new ResponseEntity<>(box.get(), HttpStatus.CREATED);
            }
            else
            {
                log.warn("adding failed");
                return new ResponseEntity<>(HttpStatus.NO_CONTENT);
            }
        }
        catch (InvalidRequestException ex)
        {
            return new ResponseEntity<>(ex.getStatusCode());
        }
    }

    /**
     * обновление информации о сообщении
     * выполнять может как пользователь, так и администратор
     * @param id
     * @param dto
     * @param principal
     * @return
     * 200 - успешное обновление сообщения
     * 204 - обнволение неудачно
     * 404 - сообщение не найдено
     * 403 - обновление не своего сообщения
     * 401 - анонимный запрос
     */
    @PutMapping("/{id}")
    @PreAuthorize("hasAnyAuthority('USER','ADMIN')")
    public ResponseEntity<MessageDto> updateMessage(@PathVariable Long id, @Valid @RequestBody MessageDto dto, Principal principal)
    {
        log.info("updating message");
        try{
            Optional<MessageDto> box = messageService.updateMessage(id, dto, principal.getName());
            if (box.isPresent())
            {
                return new ResponseEntity<>(box.get(), HttpStatus.OK);
            }
            else
            {
                log.warn("updating failed");
                return new ResponseEntity<>(HttpStatus.NO_CONTENT);
            }
        }
        catch (InvalidRequestException ex)
        {
            return new ResponseEntity<>(ex.getStatusCode());
        }
    }

    /**
     * удаление сообщения по идентификатору
     * выполнять может как пользователь, так и администратор
     * @param id
     * @param principal
     * @return
     * 200 - успешное удаление сообщения
     * 404 - сообщение не найдено
     * 403 - попытка удалить сообщение пользоваетелем, если он не является отправителем сообщения (для роли пользователя)
     * 401 - анонимный запрос
     * 400 - неверные входные параметры
     */
    @DeleteMapping("/{id}")
    @PreAuthorize("hasAnyAuthority('USER','ADMIN')")
    public ResponseEntity<Void> removeMessage(@PathVariable Long id, Principal principal)
    {
        log.info("removing message");
        try
        {
            messageService.removeMessage(id, principal.getName());
            return new ResponseEntity<>(HttpStatus.OK);
        }
        catch (InvalidRequestException ex)
        {
            return new ResponseEntity<>(ex.getStatusCode());
        }
    }

    /**
     * удаление сообщений по параметрам
     * выполнять может как пользователь, так и администратор
     * @param dto
     * @param principal
     * @return
     * 200 - успешное удаление сообщений
     * 404 - одно из сообщений не найдено (при предварительной проверке)
     * 403 - одно из сообщений, которые пользователь хочет удалить, не соответствует отправителю (при предварительной проверке)
     * 401 - анонимный запрос
     * 400 - неверные входные параметры
     */
    @PostMapping("/delete")
    @PreAuthorize("hasAnyAuthority('USER', 'ADMIN')")
    public ResponseEntity<Void> removeSeveralMessages(@Valid @RequestBody MessageSearchDto dto, Principal principal, Pageable pageable)
    {
        log.info("removing messages by parameters");
        try{
            messageService.removeMessages(dto, principal.getName(), pageable);
            return new ResponseEntity<>(HttpStatus.OK);
        }
        catch (InvalidRequestException ex)
        {
            return new ResponseEntity<>(ex.getStatusCode());
        }
    }
}
