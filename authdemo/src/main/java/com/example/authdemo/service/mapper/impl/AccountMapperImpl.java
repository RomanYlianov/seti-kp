package com.example.authdemo.service.mapper.impl;

import com.example.authdemo.model.Account;
import com.example.authdemo.model.Role;
import com.example.authdemo.service.dto.AccountDto;
import com.example.authdemo.service.dto.AccountSmartDto;
import com.example.authdemo.service.mapper.AccountMapper;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.List;


@Service
public class AccountMapperImpl implements AccountMapper {

    Logger log = LoggerFactory.getLogger(AccountMapperImpl.class);

    @Override
    public Account toEntity(AccountDto dto) {
        log.info("accountMapper: convert dto to entity");
        Account entity = new Account();
        if (dto!=null)
        {
            if (dto.getId()!=null)
                entity.setId(dto.getId());
            if (dto.getFirstName()!=null)
                entity.setFirstName(dto.getFirstName());
            if (dto.getLastName()!=null)
                entity.setLastName(dto.getLastName());
            if (dto.getEmail()!=null)
                entity.setEmail(dto.getEmail());
            if (dto.getPassword()!=null)
                entity.setPassword(dto.getPassword());
            if (dto.getRoles()!=null)
                entity.setRoles(dto.getRoles());
        }
        else
        {
            log.warn("accountMapper: input data is empty");
        }
        return entity;
    }

    @Override
    public AccountDto toDto(Account entity) {
        log.info("accountMapper: convert entity to dto");
        AccountDto dto = new AccountDto();
        if (entity!=null)
        {
            if (entity.getId()!=null)
                dto.setId(entity.getId());
            if (entity.getFirstName()!=null)
                dto.setFirstName(entity.getFirstName());
            if (entity.getLastName()!=null)
                dto.setLastName(entity.getLastName());
            if (entity.getEmail()!=null)
                dto.setEmail(entity.getEmail());
            if (entity.getPassword()!=null)
                dto.setPassword(entity.getPassword());
            if (entity.getRoles()!=null)
                dto.setRoles(entity.getRoles());
        }
        else
        {
            log.warn("accountMapper: input data is empty");
        }
        return dto;
    }

    @Override
    public List<Account> toEntity(List<AccountDto> dtoList) {
        log.info("accountMapper: convert list of dto to list of entities");
        List<Account> list = new ArrayList();
        if (dtoList!=null)
        {
            for (AccountDto dto: dtoList)
            {
                if (dto!=null)
                    list.add(toEntity(dto));
            }
        }
        else
        {
            log.warn("accountMapper: input data is empty");
        }
        return list;
    }

    @Override
    public List<AccountDto> toDto(List<Account> entities) {
        log.info("accountMapper: convert list of entities to list of dto");
        List<AccountDto> dtoList = new ArrayList();
        if (entities!=null)
        {
            for (Account entity: entities)
            {
                dtoList.add(toDto(entity));
            }
        }
        else
        {
            log.warn("accountMapper: input data is empty");
        }
        return dtoList;
    }

    @Override
    public AccountSmartDto entityToSmartDto(Account entity) {
        log.info("accountMapper: convert entity to smartDto");
        AccountSmartDto smartDto = new AccountSmartDto();
        if (entity!=null)
        {
            if (entity.getId()!=null)
                smartDto.setId(entity.getId());
            if (entity.getFirstName()!=null)
                smartDto.setFirstName(entity.getFirstName());
            if (entity.getLastName()!=null)
                smartDto.setLastName(entity.getLastName());
            if (entity.getEmail()!=null)
                smartDto.setEmail(entity.getEmail());
            if (entity.getRoles()!=null)
            {
               smartDto.setRoles(entity.getRoles());
            }
        }
        else
        {
            log.warn("accountMapper: input data is empty");
        }
        return smartDto;
    }

    @Override
    public AccountSmartDto dtoToSmartDto(AccountDto dto) {
        log.info("accountMapper: convert dto to smartDto");
        AccountSmartDto smartDto = new AccountSmartDto();
        if (dto!=null)
        {
            if (dto.getId()!=null)
                smartDto.setId(dto.getId());
            if (dto.getFirstName()!=null)
                smartDto.setFirstName(dto.getFirstName());
            if (dto.getLastName()!=null)
                smartDto.setLastName(dto.getLastName());
            if (dto.getEmail()!=null)
                smartDto.setEmail(dto.getEmail());
            if (dto.getRoles()!=null)
            {
                smartDto.setRoles(dto.getRoles());
            }
        }
        else
        {
            log.warn("accountMapper: input data is empty");
        }
        return smartDto;
    }

    @Override
    public List<AccountSmartDto> entityToSmartDto(List<Account> entities) {
        log.info("accountMapper: convert list of entities to list of smartDto");
        List<AccountSmartDto> smartDtoList = new ArrayList();
        if (entities!=null)
        {
            for (Account entity: entities)
            {
                smartDtoList.add(entityToSmartDto(entity));
            }
        }
        else
        {
            log.warn("accountMapper: input data is empty");
        }
        return smartDtoList;
    }

    @Override
    public List<AccountSmartDto> dtoToSmartDto(List<AccountDto> dtoList) {
        log.info("accountMapper: convert list of dto to list of smartDto");
        List<AccountSmartDto> smartDtoList = new ArrayList<>();
        if (dtoList!=null)
        {
            for (AccountDto dto: dtoList)
            {
                smartDtoList.add(dtoToSmartDto(dto));
            }
        }
        else
        {
            log.warn("accountMapper: input data is empty");
        }
        return smartDtoList;
    }
}
