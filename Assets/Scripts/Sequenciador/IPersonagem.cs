using UnityEngine;

namespace SSaME.Core
{
    public interface IPersonagem
    {
        Times Time { get; }
        int Id { get; }
        int Nivel { get; }

        void Inicializar(int id);
        void Selecionar();
        void Atacar();
        void SubirNivel();
        Collider GetCollider();
    }
}