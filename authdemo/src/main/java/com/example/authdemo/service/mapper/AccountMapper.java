package com.example.authdemo.service.mapper;

import com.example.authdemo.model.Account;
import com.example.authdemo.service.dto.AccountDto;
import com.example.authdemo.service.dto.AccountSmartDto;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public interface AccountMapper {

    Account toEntity(AccountDto dto);

    AccountDto toDto(Account entity);

    List<Account> toEntity(List<AccountDto> dtoList);

    List<AccountDto> toDto(List<Account> entities);

    AccountSmartDto entityToSmartDto(Account entity);

    AccountSmartDto dtoToSmartDto(AccountDto dto);

    List<AccountSmartDto> entityToSmartDto(List<Account> entities);

    List<AccountSmartDto> dtoToSmartDto(List<AccountDto> dtoList);

}
