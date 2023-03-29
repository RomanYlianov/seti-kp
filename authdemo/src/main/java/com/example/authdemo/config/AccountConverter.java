package com.example.authdemo.config;

import com.example.authdemo.model.Account;
import com.example.authdemo.repository.AccountRepository;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.core.convert.converter.Converter;
import org.springframework.stereotype.Component;

import java.util.Optional;

@Component
public class AccountConverter implements Converter<String, Account> {

    Logger log = LoggerFactory.getLogger(AccountConverter.class);

    private final AccountRepository accountRepository;

    @Autowired
    public AccountConverter(AccountRepository accountRepository) {
        this.accountRepository = accountRepository;
    }

    @Override
    public Account convert(String source) {
        log.info("accountConverter: convert string to account entity");
        Account entity = new Account();
        if (!source.isEmpty())
        {
            try {
                Long id = Long.parseLong(source);
                Optional<Account> box = accountRepository.findById(id);
                if (box.isPresent())
                {
                    entity = box.get();
                }
                else
                {
                    log.warn("account with id "+source+" was not found");
                }
            }
            catch (NumberFormatException ex)
            {
                log.warn("id must be numeric value");
            }
        }
        return entity;
    }
}
