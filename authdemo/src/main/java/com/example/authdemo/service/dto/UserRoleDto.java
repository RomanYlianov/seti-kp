package com.example.authdemo.service.dto;

import com.example.authdemo.model.Role;
import lombok.Data;

import javax.validation.constraints.NotNull;
import javax.validation.constraints.Positive;

@Data
public class UserRoleDto {

    @NotNull(message = "accountId is mandatory")
    @Positive(message = "id must be positive")
    private Long accountId;

    @NotNull(message = "roleName is mandatory")
    private Role role;
}
