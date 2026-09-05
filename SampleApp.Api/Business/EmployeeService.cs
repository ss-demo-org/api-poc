namespace SampleApp.Api.Business
{
    public class EmployeeService : IEmployeeService
    {

       /* public EmployeeService(IEmployeeManagementRepository repository, EmployeeFactory employeeFactory)
        {
            _repository = repository;
            _employeeFactory = employeeFactory;
        }
        public async Task<InternalEmployee?> FetchInternalEmployeeAsync(Guid employeeId)
        {
            var employee = await _repository.GetInternalEmployeeAsync(employeeId);

            if (employee != null)
            {
                // calculate fields
                employee.SuggestedBonus = CalculateSuggestedBonus(employee);
            }
            return employee;
        }*/
    }
}
