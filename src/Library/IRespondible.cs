namespace Library
{
    public interface IRespondible
    {
        bool Respondido { get; }
        void MarcarComoRespondido();
        
        bool EsRespuestaDe(IRespondible otraInteraccion);


    }

}