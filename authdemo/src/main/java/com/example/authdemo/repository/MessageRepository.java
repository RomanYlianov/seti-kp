package com.example.authdemo.repository;

import com.example.authdemo.model.Message;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.repository.PagingAndSortingRepository;
import org.springframework.stereotype.Repository;

import java.util.Date;

@Repository
public interface MessageRepository extends PagingAndSortingRepository<Message, Long> {

    Page<Message> findBySender_emailContainingAndReceiver_emailContainingAndMessageTextContaining(String senderUsername, String receiverUsername, String messageText, Pageable pageable);

}
