package com.example.authdemo.service.impl;

import com.example.authdemo.model.Account;
import com.example.authdemo.repository.AccountRepository;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.security.core.userdetails.UserDetailsService;
import org.springframework.security.core.userdetails.UsernameNotFoundException;
import org.springframework.stereotype.Service;

import java.util.Optional;

@Service
public class UserDetailsServiceImpl implements UserDetailsService {

    private Logger log = LoggerFactory.getLogger(UserDetailsServiceImpl.class);

    @Autowired
    private AccountRepository accountRepository;

    @Override
    public UserDetails loadUserByUsername(String username) throws UsernameNotFoundException {
        log.info("load account with username "+username);
        Account entity = null;
        if (username!=null)
        {
            Optional<Account> box = accountRepository.findByEmail(username);
            if (box.isPresent())
            {
                entity = box.get();
            }
            else
            {
                log.warn("account with email "+username+" was not found");
            }
        }
        return entity;
    }
}
