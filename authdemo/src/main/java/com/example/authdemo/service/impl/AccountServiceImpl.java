package com.example.authdemo.service.impl;

import com.example.authdemo.config.InvalidRequestException;
import com.example.authdemo.model.Account;
import com.example.authdemo.model.Role;
import com.example.authdemo.repository.AccountRepository;
import com.example.authdemo.service.AccountService;
import com.example.authdemo.service.dto.AccountDto;
import com.example.authdemo.service.dto.AccountSearchDto;
import com.example.authdemo.service.dto.AccountSmartDto;
import com.example.authdemo.service.mapper.AccountMapper;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Pageable;
import org.springframework.http.HttpStatus;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import java.util.*;

@Service
public class AccountServiceImpl  implements AccountService {

    private Logger log = LoggerFactory.getLogger(AccountServiceImpl.class);


    private AccountRepository accountRepository;

    private AccountMapper accountMapper;

    private PasswordEncoder passwordEncoder;

    @Autowired
    public AccountServiceImpl(AccountRepository accountRepository, AccountMapper accountMapper, PasswordEncoder passwordEncoder) {
        this.accountRepository = accountRepository;
        this.accountMapper = accountMapper;
        this.passwordEncoder = passwordEncoder;
    }

    @Transactional
    @Override
    public List<AccountSmartDto> searchAccount(AccountSearchDto dto, String username, Pageable pageable) throws InvalidRequestException {
        log.info("search account by parameters");
        List<AccountSmartDto>smartDtoList = new ArrayList<>();
        if (dto!=null && username!=null)
        {
            Page<Account> page = accountRepository.findByFirstNameContainingAndLastNameContainingAndEmailContaining(dto.getFirstName(), dto.getLastName(), dto.getEmail(), pageable);
            Optional<Account> box = accountRepository.findByEmail(username);
            Boolean isAdmin = false;
            if (box.isPresent())
            {
                isAdmin = box.get().getRoles().contains(Role.ADMIN);
            }
            if (!page.isEmpty())
            {
                for (Account entity: page.toList())
                {
                    if (entity.getRoles().containsAll(dto.getRoles()))
                    {
                        if (!isAdmin && entity.getEmail().equals(username))
                        {
                            smartDtoList.add(accountMapper.entityToSmartDto(entity));
                        }
                        else
                        {
                            smartDtoList.add(accountMapper.entityToSmartDto(entity));
                        }
                    }
                }
                return smartDtoList;
            }
            else
            {
                String message = "accounts was not found, please check search parameters and try again";
                log.warn(message);
                throw new InvalidRequestException(HttpStatus.NOT_FOUND, message);
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
    public Optional<AccountSmartDto> add(AccountDto dto) throws InvalidRequestException {
        log.info("adding account");
        if (dto!=null)
        {
            if (accountRepository.findByEmail(dto.getEmail()).isPresent())
            {
                String message = "account with email "+dto.getEmail()+" is already register in the system";
                log.warn(message);
                throw new InvalidRequestException(HttpStatus.CONFLICT, message);
            }
            Set<Role> roles = new HashSet<>();
            dto.setPassword(passwordEncoder.encode(dto.getPassword()));
            roles.add(Role.USER);
            dto.setRoles(roles);
            dto = accountMapper.toDto(accountRepository.save(accountMapper.toEntity(dto)));
            return Optional.ofNullable(accountMapper.dtoToSmartDto(dto));
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
    public Optional<AccountSmartDto> update(Long id, AccountDto dto, String username) throws InvalidRequestException{
        log.info("updating account");
        if (id!=null && dto!=null && username!=null)
        {
            if (id>0)
            {
                if (!dto.getEmail().matches("^(.+)@(.+)$"))
                {
                    String message = "email is not valid";
                    log.warn(message);
                    throw new InvalidRequestException(HttpStatus.BAD_REQUEST, message);
                }
                if (accountRepository.findByEmail(dto.getEmail()).isPresent())
                {
                    String message = "account with email "+dto.getEmail()+" is already register in the system";
                    log.warn(message);
                    throw new InvalidRequestException(HttpStatus.CONFLICT, message);
                }
                Optional<Account> box = accountRepository.findById(id);
                Optional<Account> currentUserBox = accountRepository.findByEmail(username);
                if (box.isPresent())
                {
                    Account entity = box.get();
                    Account currentUser = currentUserBox.get();
                    if (!entity.getEmail().equals(username) && !currentUser.getRoles().contains(Role.ADMIN))
                    {
                        String message = "trying to update is not yourself account";
                        log.warn(message);
                        throw new InvalidRequestException(HttpStatus.FORBIDDEN, message);
                    }
                    if (!entity.getRoles().contains(Role.ADMIN) && dto.getRoles().contains(Role.ADMIN))
                    {
                        String message = "only admin can add \"ADMIN\" role";
                        log.warn(message);
                        throw new InvalidRequestException(HttpStatus.FORBIDDEN, message);
                    }
                    dto.setId(id);
                    dto.setPassword(passwordEncoder.encode(dto.getPassword()));
                    entity = accountMapper.toEntity(dto);
                    entity = accountRepository.save(entity);
                    return Optional.ofNullable(accountMapper.entityToSmartDto(entity));
                }
                else
                {
                    String message = "account with id "+dto.getId()+" was not found";
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
    public void remove(Long id, String username) throws InvalidRequestException{
        log.info("removing account");
        if (id!=null)
        {
            if (id>0)
            {
                Optional<Account> box = accountRepository.findById(id);
                Optional<Account> currentUserBox = accountRepository.findByEmail(username);
                if (box.isPresent())
                {
                    Account entity = box.get();
                    Account currentUser = currentUserBox.get();
                    if (!currentUser.getRoles().contains(Role.ADMIN) && !entity.getId().equals(currentUser.getId()))
                    {
                        String message = "trying to remove is not yousrlf account";
                        log.warn(message);
                        throw new InvalidRequestException(HttpStatus.FORBIDDEN, message);
                    }
                }
                if (accountRepository.findById(id).isPresent())
                {
                    accountRepository.deleteById(id);
                }
                else
                {
                    String message = "account with id "+id+" was not found";
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
    public Set<Role> getAllRolesForAccount(Long accountId, String username) throws InvalidRequestException {
        log.info("getting all roles for account");
        if (accountId!=null)
        {
            if (accountId>0)
            {
                Optional<Account> box = accountRepository.findById(accountId);
                Optional<Account> currentUserBox = accountRepository.findByEmail(username);
                if (box.isPresent())
                {
                    Account entity = box.get();
                    Account currentUser = currentUserBox.get();
                    if (!currentUser.getId().equals(accountId) && !currentUser.getRoles().contains(Role.ADMIN))
                    {
                        String message = "trying to get information not about your account";
                        log.warn(message);
                        throw new InvalidRequestException(HttpStatus.FORBIDDEN, message);
                    }
                    return box.get().getRoles();
                }
                else
                {
                    String message = "account with id "+accountId+" was not found";
                    log.warn(message);
                    throw new InvalidRequestException(HttpStatus.NOT_FOUND, message);
                }
            }
            else
            {
                String message = "accountId must be positive";
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
    public Optional<AccountSmartDto> addRoleToAccount(Long accountId, Role role) throws InvalidRequestException {
        log.info("adding role to account");
        if (accountId!=null && role!=null)
        {
            if (accountId>0)
            {
                Optional<Account> box = accountRepository.findById(accountId);
                if (box.isPresent())
                {
                    Account entity = box.get();
                    Set<Role> roles = entity.getRoles();
                    try {
                        roles.add(role);
                    }
                    catch (IllegalStateException e)
                    {
                        String message = "user with id "+accountId+" already have role "+ role.name();
                        log.warn(message);
                        throw new InvalidRequestException(HttpStatus.CONFLICT, message);
                    }
                    entity.setRoles(roles);
                    entity = accountRepository.save(entity);
                    return Optional.ofNullable(accountMapper.entityToSmartDto(entity));
                }
                else
                {
                    String message = "account with id "+accountId+" was not found";
                    log.warn(message);
                    throw new InvalidRequestException(HttpStatus.NOT_FOUND, message);
                }
            }
            else
            {
                String message = "accountId must be positive";
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
    public Optional<AccountSmartDto> removeRoleForAccount(Long accountId, Role role) throws InvalidRequestException {
        log.info("removing role for account");
        if (accountId!=null && role!=null)
        {
            if (accountId>0)
            {
                Optional<Account> box = accountRepository.findById(accountId);
                if (box.isPresent())
                {
                    Account entity = box.get();
                    Set<Role> roles = entity.getRoles();
                    if (roles.contains(role))
                    {
                        roles.remove(role);
                        entity.setRoles(roles);
                        entity = accountRepository.save(entity);
                        return Optional.ofNullable(accountMapper.entityToSmartDto(entity));
                    }
                    else
                    {
                        String message = "account with id "+accountId+" is not contains role "+role.name();
                        log.warn(message);
                        throw new InvalidRequestException(HttpStatus.NOT_FOUND, message);
                    }
                }
                else
                {
                    String message = "account with id "+accountId+" was not found";
                    log.warn(message);
                    throw new InvalidRequestException(HttpStatus.NOT_FOUND, message);
                }
            }
            else
            {
                String message = "accountId must be positive";
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

}
