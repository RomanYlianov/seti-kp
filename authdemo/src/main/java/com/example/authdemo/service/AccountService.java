package com.example.authdemo.service;

import com.example.authdemo.config.InvalidRequestException;
import com.example.authdemo.model.Role;
import com.example.authdemo.service.dto.AccountDto;
import com.example.authdemo.service.dto.AccountSearchDto;
import com.example.authdemo.service.dto.AccountSmartDto;
import org.springframework.data.domain.Pageable;
import org.springframework.stereotype.Service;
import java.util.List;
import java.util.Optional;
import java.util.Set;

@Service
public interface AccountService {

    List<AccountSmartDto> searchAccount(AccountSearchDto dto, String username, Pageable pageable) throws InvalidRequestException;

    Optional<AccountSmartDto> add(AccountDto dto) throws InvalidRequestException;

    Optional<AccountSmartDto> update(Long id,AccountDto dto, String username) throws InvalidRequestException;

    void remove(Long id, String username) throws InvalidRequestException;

    Set<Role> getAllRolesForAccount(Long accountId, String username) throws InvalidRequestException;

    Optional<AccountSmartDto> addRoleToAccount(Long accountId, Role role) throws InvalidRequestException;

    Optional<AccountSmartDto> removeRoleForAccount(Long accountId, Role role) throws InvalidRequestException;

}
