﻿using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DotNetUEApp_C_.Services;

public class JwtService
{
    public String SecretKey { get; set; }
    public int TokenDuration { get; set; }

    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
        this.SecretKey = _configuration.GetSection("jwtConfig").GetSection("Key").Value;
        this.TokenDuration = Int32.Parse(_configuration.GetSection("jwtConfig").GetSection("Duration").Value);
    }

    public String GenerateToken(String id, String firstname, String lastname, String email)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.SecretKey));
        
        var signature = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var payload = new[]
        {
             new Claim("id", id),
             new Claim("firstname", firstname),
             new Claim("lastname", lastname),
             new Claim("email", email)
        };

        var jwtToken = new JwtSecurityToken(
            issuer: "localhost",
            audience: "localhost",
            claims: payload,
            expires: DateTime.Now.AddMinutes(TokenDuration),
            signingCredentials: signature

            );

        return new JwtSecurityTokenHandler().WriteToken(jwtToken);

    }  
}
