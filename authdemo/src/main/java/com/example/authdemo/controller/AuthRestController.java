package com.example.authdemo.controller;

import com.example.authdemo.config.InvalidRequestException;
import com.example.authdemo.model.Account;
import com.example.authdemo.service.AccountService;
import com.example.authdemo.service.dto.AccountDto;
import com.example.authdemo.service.dto.AccountSmartDto;
import com.example.authdemo.service.mapper.AccountMapper;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.security.core.userdetails.UserDetailsService;
import org.springframework.security.web.authentication.WebAuthenticationDetails;
import org.springframework.web.bind.annotation.*;

import javax.servlet.http.HttpServletRequest;
import javax.validation.Valid;
import java.security.Principal;
import java.util.Optional;

@RestController
public class AuthRestController {

    private Logger log = LoggerFactory.getLogger(AuthRestController.class);

    private AccountService accountService;

    private AccountMapper accountMapper;

    private UserDetailsService userDetailsService;

    private AuthenticationManager authenticationManager;

    @Autowired
    public AuthRestController(AccountService accountService, AccountMapper accountMapper, UserDetailsService userDetailsService, AuthenticationManager authenticationManager) {
        this.accountService = accountService;
        this.accountMapper = accountMapper;
        this.userDetailsService = userDetailsService;
        this.authenticationManager = authenticationManager;
    }

    @GetMapping("/get-example")
    public String getExample()
    {
        return "working";
    }

    @PostMapping("/post-example")
    public String postExample(@RequestBody String name)
    {
        return name;
    }

    /**
     * метод-заглушка для авторизации пользователей в системе
     * @return авторизованный аккаунт или null
     */
    @PostMapping("/login")
    public@ResponseBody AccountSmartDto login(Principal principal, HttpServletRequest request)
    {
        log.info("enter in account");
        request.getSession().invalidate();
        if (principal!=null)
        {
            log.info("load user information");
            Account user = (Account) userDetailsService.loadUserByUsername(principal.getName());
            return accountMapper.entityToSmartDto(user);

        }
        else
        {
            log.warn("login failed");
             return null;
        }
    }

    @GetMapping("/user-info")
    public ResponseEntity<AccountSmartDto> getCurrent(Principal principal)
    {
        if (principal!=null)
        {
            Account user = (Account) userDetailsService.loadUserByUsername(principal.getName());
            AccountSmartDto dto = accountMapper.entityToSmartDto(user);
            return new ResponseEntity<>(dto, HttpStatus.OK);
        }
        else
        {
            return new ResponseEntity<>(null, HttpStatus.OK);
        }
    }

    /**
     * регистрация пользователей в системе
     * @param dto
     * @return
     * 201 - успешная регистрация
     * 400 - неверные входные параметры
     * 204 - авторизация не произведена (NO_CONTENT)
     * 409 - аккаунт уже зарегистрирован
     */
    @PostMapping("/registration")
    public ResponseEntity<AccountSmartDto> register(@Valid @RequestBody AccountDto dto, Principal principal, HttpServletRequest request)
    {
        log.info("register new account");
        String password = dto.getPassword();
        try
        {
            Optional<AccountSmartDto> box = accountService.add(dto);
            if (box.isPresent())
            {
                AccountSmartDto smartDto = box.get();
                log.info("register success, created account id is {}", box.get().getId());
                log.info("login in the system");
                authenticateUserAndSetSession(dto.getEmail(), password, request);
                String authUserName = SecurityContextHolder.getContext().getAuthentication().getName();
                if (authUserName!=null)
                {
                    log.info("success authorize user with email "+authUserName);
                    return new ResponseEntity(smartDto, HttpStatus.CREATED);
                }
                else
                {
                    log.warn("authorization failed");
                    return new ResponseEntity<>(HttpStatus.NO_CONTENT);
                }
            }
            else
            {
                log.error("register failed");
                return new ResponseEntity(HttpStatus.NO_CONTENT);
            }


        }
        catch (InvalidRequestException ex)
        {
            return new ResponseEntity<>(ex.getStatusCode());
        }
    }

    /**
     * метод-заглушка, обрабатывает выход пользователя из системы
     * @param request
     * @return
     * 200 - успешный выход
     */
    @PostMapping("/logout")
    public ResponseEntity<Void> logout(HttpServletRequest request)
    {
        log.info("clear session");
        request.getSession().invalidate();
        log.info("logout success");
        return new ResponseEntity<>(HttpStatus.OK);
    }

    private void authenticateUserAndSetSession(String username, String password, HttpServletRequest request)
    {
        log.info("#1. username {}, password {}",username, password);
        UsernamePasswordAuthenticationToken token = new UsernamePasswordAuthenticationToken(username, password);
        Authentication authenticatedUser = authenticationManager.authenticate(token);
        token.setDetails(new WebAuthenticationDetails(request));
        SecurityContextHolder.getContext().setAuthentication(authenticatedUser);
    }
}
