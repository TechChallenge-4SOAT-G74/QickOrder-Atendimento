namespace QuickOrderAtendimento.Infra.MQ
{
    public interface IProcessaEvento
    {
        void Processa(string mensagem);
    }
}
