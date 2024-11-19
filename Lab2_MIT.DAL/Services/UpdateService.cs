using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Transactions;
using Lab2_MIT.DAL.Models; 
using Lab2_MIT.DAL.Services; 

namespace Lab2_MIT.DAL.Services
{
    public class UpdateService : IHostedService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public UpdateService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await using (var scope = _serviceScopeFactory.CreateAsyncScope())
            {
                var mongoService = scope.ServiceProvider.GetRequiredService<StudentService>(); // без using
                var dbContext = scope.ServiceProvider.GetRequiredService<LabDbContext>();
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        var students = dbContext.Students.ToList();
                        foreach (var student in students)
                        {
                            var studentDto = new StudentDto()
                            {
                                Name = student.FirstName + " " + student.LastName, // Замість Name
                                Birth = student.BirthDate
                            };
                            studentDto.Id = studentDto.Name + studentDto.Birth.ToLongDateString();
                            await mongoService.CreateAsync(studentDto);
                        }
                        transaction.Complete();
                    }
                    catch (Exception ex)
                    {
                        transaction.Dispose();
                        throw;
                    }
                }
            }
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
