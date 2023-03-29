package com.example.authdemo;

import com.example.authdemo.model.Account;
import com.example.authdemo.repository.AccountRepository;
import org.junit.jupiter.api.Test;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.util.Assert;

@SpringBootTest
class AuthdemoApplicationTests {

	Logger log = LoggerFactory.getLogger(AuthdemoApplicationTests.class);

	@Autowired
	private AccountRepository accountRepository;

	@Test
	void contextLoads() {
		Page<Account> page = accountRepository.findByFirstNameContainingAndLastNameContainingAndEmailContaining("i","i","u", Pageable.unpaged());
		Assert.isTrue(page.getSize()>0);
	}

}
