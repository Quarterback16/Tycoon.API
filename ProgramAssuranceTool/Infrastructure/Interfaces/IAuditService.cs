namespace ProgramAssuranceTool.Infrastructure.Interfaces
{
	public interface IAuditService
	{
		void AuditActivity( string activity, string userId );
	}
}