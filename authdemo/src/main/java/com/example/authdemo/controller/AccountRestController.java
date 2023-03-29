package com.example.authdemo.controller;

import com.example.authdemo.config.InvalidRequestException;
import com.example.authdemo.model.Role;
import com.example.authdemo.service.AccountService;
import com.example.authdemo.service.dto.AccountDto;
import com.example.authdemo.service.dto.AccountSearchDto;
import com.example.authdemo.service.dto.AccountSmartDto;
import com.example.authdemo.service.dto.UserRoleDto;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Pageable;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.web.bind.annotation.*;
import javax.servlet.http.HttpServletRequest;
import javax.validation.Valid;
import java.security.Principal;
import java.util.List;
import java.util.Optional;
import java.util.Set;

@RestController
@RequestMapping("/accounts")
public class AccountRestController {


    private Logger log = LoggerFactory.getLogger(AccountRestController.class);

    private AccountService accountService;

    @Autowired
    public AccountRestController(AccountService accountService) {
        this.accountService = accountService;
    }


    /**
     * Поиск аккаунтов по параметрам: часть имени, часть фамилии, часть адреса электронной почты
     * может быть выполнен авторизованный пользователем с ролью администратора или пользователя
     * при подаче null в пареметре фильтрация по данному критерию произведена не будет
     * @param dto
     * @param principal
     * @param pageable
     * @return
     * 200 - найдена одна или более записей,
     * 404 - не найдено ни одной записи,
     * 400 - неверные параметры запроса
     * 401 - анонимный запрос, запрос от пользователя
     */
    @GetMapping("/search")
    @PreAuthorize("hasAnyAuthority('ADMIN','USER')")
    public ResponseEntity<List<AccountSmartDto>> searchAccounts(@Valid AccountSearchDto dto, Principal principal, Pageable pageable)
    {
        log.info("search account by parameters");
        try
        {
            List<AccountSmartDto> list = accountService.searchAccount(dto, principal.getName(), pageable);
            return new ResponseEntity<>(list, HttpStatus.OK);
        }
        catch (InvalidRequestException e)
        {
            return new ResponseEntity<>(e.getStatusCode());
        }
    }



    /**
     * обновление информации об аккаунте
     * может быть выполнен авторизованным пользователем с ролью администратора или пользователя
     * для пользователя возможно обновление только своего аккаунта, для администратора - любого
     * @param dto
     * @return
     * 200 - успешное обновление,
     * 409 - аккаунт с таким email уже существует,
     * 404 - аккаунт с id не найден,
     * 403 - обновление не своего аккаунта,
     * 401 - анонимный запрос
     * 400 - неверные входные параметры
     */
    @PutMapping("/{id}")
    @PreAuthorize("hasAnyAuthority('USER', 'ADMIN')")
    public ResponseEntity<AccountSmartDto> updateAccount(@PathVariable Long id, @Valid @RequestBody AccountDto dto, Principal principal)
    {
        log.info("update account");
        try
        {
            Optional<AccountSmartDto> box = accountService.update(id,dto, principal.getName());
            return new ResponseEntity<>(box.get(), HttpStatus.OK);
        }
        catch (InvalidRequestException ex)
        {
            return new ResponseEntity<>(ex.getStatusCode());
        }
    }

    /**
     * удаление аккаунта по идентификатору
     * может быть выполнен авторизованным пользователем с ролью администратора или пользователя
     * для пользователя возможно удаление только своего аккаунта, для администратора - любого
     * @param id
     * @return
     * 200 - успешное удаление,
     * 404 - аккаунтне найден,
     * 403 - удаление не своего аккаунта (для пользователя),
     * 401 - анонимный запрос
     * 400 - неверные входные параметры
     */
    @DeleteMapping("/{id}")
    @PreAuthorize("hasAnyAuthority('USER', 'ADMIN')")
    public ResponseEntity<Void> removeAccount(@PathVariable Long id, Principal principal, HttpServletRequest request)
    {
        log.info("remove account");
        try
        {
            accountService.remove(id, principal.getName());
            request.getSession().invalidate();
            return new ResponseEntity<>(HttpStatus.OK);
        }
        catch (InvalidRequestException ex)
        {
            return new ResponseEntity<>(ex.getStatusCode());
        }
    }

    /**
     * получение всех ролей пользователя
     * может быть выполнен авторизованным пользователем с ролью администратора или пользователя
     * для пользователя возможен просмотр только своих ролей, для администратора - ролей любых пользователей
     * @param id
     * @return
     * 200 - успешное получение
     * 403 - получение не своих ролей (для пользователя)
     * 404 - пользователь не найден
     * 400 - неверные входные параметры
     * 401 - анонимный запрос
     */
    @GetMapping("{id}/roles")
    @PreAuthorize("hasAnyAuthority('ADMIN', 'USER')")
    public ResponseEntity<String> getRoles(@PathVariable Long id, Principal principal)
    {
        log.info("get roles for account");
        try
        {
            Set<Role> roles = accountService.getAllRolesForAccount(id, principal.getName());
            return new ResponseEntity(roles, HttpStatus.OK);
        }
        catch (InvalidRequestException ex)
        {
            return new ResponseEntity<>(ex.getStatusCode());
        }
    }

    /**
     * добавление роли пользователю
     * может быть выполнено только администратором
     * @param dto
     * @return
     * 200 - успешное добавление
     * 409 - аккаунт уже имеет роль
     * 404 - пользователь не найден
     * 401 - анонимный запрос, запрос от пользователя
     * 400 - неверные входные параметры
     */
    @PostMapping("/add-role")
    @PreAuthorize("hasAnyAuthority('ADMIN')")
    public ResponseEntity<AccountSmartDto> addRoleForAccount(@Valid @RequestBody UserRoleDto dto)
    {
        log.info("adding role to account");
        try
        {
            Optional<AccountSmartDto> box = accountService.addRoleToAccount(dto.getAccountId(), dto.getRole());
            return new ResponseEntity<>(box.get(), HttpStatus.OK);
        }
        catch (InvalidRequestException ex)
        {
            return new ResponseEntity<>(ex.getStatusCode());
        }
    }

    /**
     * удалениие роли пользователя
     * может быть выполнено только администратором
     * @param dto
     * @return
     * 200 - успешное удаление
     * 404 - пользователь не найден, роль у пользователя не найдена
     * 401 - анонимный запрос, запрос от пользователя
     * 400 - неверные входные параметры
     */
   /* @PostMapping("/remove-role")
    @PreAuthorize("hasAnyAuthority('ADMIN')")
    public ResponseEntity<AccountSmartDto> removeRoleForAccount(@Valid @RequestBody UserRoleDto dto)
    {
        log.warn("removing role for account");
        try
        {
            Optional<AccountSmartDto> box = accountService.removeRoleForAccount(dto.getAccountId(), dto.getRole());
            return new ResponseEntity<>(box.get(), HttpStatus.OK);
        }
        catch (InvalidRequestException ex)
        {
            return new ResponseEntity<>(ex.getStatusCode());
        }
    }*/

    @PostMapping("/remove-role")
    @PreAuthorize("hasAnyAuthority('ADMIN')")
    public ResponseEntity<AccountSmartDto> removeRoleForAccount(@Valid @RequestBody UserRoleDto dto)
    {
        log.info("adding role to account");
        try
        {
            Optional<AccountSmartDto> box = accountService.removeRoleForAccount(dto.getAccountId(), dto.getRole());
            return new ResponseEntity<>(box.get(), HttpStatus.OK);
        }
        catch (InvalidRequestException ex)
        {
            return new ResponseEntity<>(ex.getStatusCode());
        }
    }

   /* @ExceptionHandler(com.fasterxml.jackson.core.JsonParseException.class)
    public void handleException(com.fasterxml.jackson.core.JsonParseException ex) {
        log.error("Error parse json " +  ex.getMessage());
    }*/
}
