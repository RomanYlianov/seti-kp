package com.example.authdemo.repository;

import com.example.authdemo.model.Account;
import com.example.authdemo.model.Role;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.repository.PagingAndSortingRepository;
import org.springframework.stereotype.Repository;
import java.util.Optional;

@Repository
public interface AccountRepository extends PagingAndSortingRepository<Account, Long>{

   Page<Account> findByFirstNameContainingAndLastNameContainingAndEmailContaining(String firstName, String lastName, String email, Pageable pageable);

    Optional<Account> findByEmail(String name);
}
