using Microsoft.EntityFrameworkCore;
using Stefanini.Domain.Test.Fixture.User;
using Stefanini.Infrastructure.Context;
using Stefanini.Infrastructure.Repositories.User;

namespace Stefanini.Infrastructure.Test.Repositories.User;

public class UserRepositoryTest
{
    private readonly UserRepository _userRepository;
    private readonly UnitOfWork.UnitOfWork _unitOfWorkMoq;
    private readonly DatabaseContext _context;
    public UserRepositoryTest()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(databaseName: "DocumentTestDb")
            .Options;
        
        _context = new DatabaseContext(options);
        _userRepository = new Infrastructure.Repositories.User.UserRepository(_context);
        _unitOfWorkMoq = new UnitOfWork.UnitOfWork(_context);
        
    }
    [Fact]
    public async Task Must_Create_An_Client_On_Database()
    {
        var userMoq = Domain.Entities.User.Create(UserFixture.Email, UserFixture.Password);
        _userRepository.Create(userMoq);
        await _unitOfWorkMoq.Commit();
        
        var user = await _userRepository.FindByEmailAsync(userMoq.Email);
        
        Assert.NotNull(user);
        
        Assert.Equal(userMoq.Email, user.Email);
        
    }

    [Fact]
    public async Task Cannot_Create_The_User_When_Email_Already_Exists()
    {
        await Must_Create_An_Client_On_Database();
        var user = await _userRepository.FindByEmailAsync(UserFixture.Email);
        
        Assert.NotNull(user);
        
        Assert.Equal(UserFixture.Email, user.Email);
    }
}

