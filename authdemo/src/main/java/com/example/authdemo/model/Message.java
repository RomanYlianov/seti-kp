package com.example.authdemo.model;


import lombok.Data;

import javax.persistence.*;

@Data
@Entity
@Table(name = "messages")
public class Message extends DateTime{

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @ManyToOne
    @JoinColumn(name = "sender",nullable = false)
    private Account sender;

    @ManyToOne
    @JoinColumn(name = "receiver", nullable = false)
    private Account receiver;

    @Column(name = "message_text",nullable = false)
    private String messageText;


}
