package com.example.authdemo.model;

import lombok.Data;

import javax.persistence.*;
import java.util.Date;

@Data
@MappedSuperclass
public class DateTime {

    @Column(name = "creation_datetime", nullable = false)
    private Date creationTime;

    @Column(name = "modification_datetime", nullable = false)
    private Date modificationTime;
}
