package com.example.authdemo.service.dto;

import com.example.authdemo.model.Role;
import lombok.Data;
import javax.validation.constraints.NotEmpty;
import javax.validation.constraints.NotNull;
import java.util.Set;

@Data
public class AccountSearchDto {

    @NotEmpty(message = "firstName is mandatory")
    private String firstName;


    @NotEmpty(message = "lastName is mandatory")
    private String lastName;


    @NotNull(message = "user must be contains one of more roles")
    private Set<Role> roles;


    @NotEmpty(message = "email is mandatory")
    private String email;
}
