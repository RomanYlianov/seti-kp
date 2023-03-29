package com.example.authdemo.config;

import com.example.authdemo.model.Role;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.stereotype.Component;

import javax.persistence.AttributeConverter;
import java.util.HashSet;
import java.util.Set;

@Component
public class AccountRolesConverter implements AttributeConverter<Set<Role>, String> {

    private Logger log = LoggerFactory.getLogger(AccountRolesConverter.class);

    @Override
    public String convertToDatabaseColumn(Set<Role> roles) {
        log.info("accountRolesConverter: convert set of roles to string");
        String rolesString = "";
        if (roles!=null)
        {
            for (Role role: roles)
            {
                rolesString+=role.name()+",";
            }
            rolesString = rolesString.substring(0, rolesString.length()-1);
        }
        return rolesString;
    }

    @Override
    public Set<Role> convertToEntityAttribute(String rolesString) {
        log.info("accountRolesConverter: convert string to set of roles");
        Set<Role> roles = new HashSet();
        if (rolesString!=null)
        {
            String[] array = rolesString.split(",");
            for (String role: array)
            {
                roles.add(Role.valueOf(role));
            }
        }
        return roles;
    }
}
