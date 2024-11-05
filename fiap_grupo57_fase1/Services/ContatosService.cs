﻿using fiap_grupo57_fase1.Helpers;
using fiap_grupo57_fase1.Infrastructures.Excpetion;
using fiap_grupo57_fase1.Interfaces.Models.Utils;
using fiap_grupo57_fase1.Interfaces.Repositories;
using fiap_grupo57_fase1.Interfaces.Services;
using fiap_grupo57_fase1.Mappers;
using fiap_grupo57_fase1.Models.Entities;
using fiap_grupo57_fase1.Models.Enums;
using fiap_grupo57_fase1.Models.Requests;
using fiap_grupo57_fase1.Models.Responses;
using System.Net;

namespace fiap_grupo57_fase1.Services
{
    public class ContatosService : IContatosService
    {
        private readonly IContatosRepository _contatosRepository;
        private readonly IObterRegiaoPorDDD _obterRegiaoPorDDD;

        public ContatosService(IContatosRepository contatosRepository, IObterRegiaoPorDDD obterRegiaoPorDDD)
        {
            _contatosRepository = contatosRepository;
            _obterRegiaoPorDDD = obterRegiaoPorDDD;
        }

        public async Task<ContatosGetResponse> ObterContatoPorId(int id)
        {
            await ValidaIdContato(id);

            return await _contatosRepository.ObterContatoPorId(id);
        }

        public List<ContatosGetResponse> ObterContatos(int ddd, string? regiao)
        {
            if (!string.IsNullOrWhiteSpace(regiao) && !Enum.TryParse(regiao, true, out RegiaoEnum regiaoEnum))
                throw new CustomException(HttpStatusCode.BadRequest, "A região fornecida é inválida.");


            if (string.IsNullOrWhiteSpace(regiao))
            {
                var result = _contatosRepository.ObterPorDDD(ddd);

                if (result == null || result.Count == 0)
                    throw new CustomException(HttpStatusCode.NotFound, "Contato não encontrado");
                else
                    return result;
            }
            else
            {
                var result = _contatosRepository.ObterPorDDDRegiao(ddd, (RegiaoEnum)Enum.Parse(typeof(RegiaoEnum), regiao));

                if (result == null || result.Count == 0)
                    throw new CustomException(HttpStatusCode.NotFound, "Contato não encontrado");
                else
                    return result;
            }
        }

        public async Task<ContatosPostResponse> AdicionarContato(ContatosPostRequest contato)
        {
            ValidatorHelper.Validar(contato);

            if (await _contatosRepository.ContatoExiste(contato))
                throw new CustomException(HttpStatusCode.Conflict, "Contato com este email já existe.");

            string regiao = _obterRegiaoPorDDD.ObtemRegiaoPorDDD(contato.DDD);

            if (regiao.Equals("DDD_INVALIDO"))
            {
                throw new CustomException(HttpStatusCode.BadRequest, $"Região NÃO ENCONTRADA para o DDD: {contato.DDD}");
            }
            contato.Regiao = regiao;

            ContatoEntity mapper = ContatoMapper.ToEntity(contato);

            int id = await _contatosRepository.Adicionar(mapper);

            return new ContatosPostResponse { Id = id };
        }

        public async Task AtualizarContato(ContatosPutRequest contato)
        {
            ValidatorHelper.Validar(contato);

            await ValidaIdContato(contato.Id);

            var contatoAux = await _contatosRepository.ObterContatoPorId(contato.Id);

            if (contatoAux.DDD != contato.DDD)
            {
                string regiao = _obterRegiaoPorDDD.ObtemRegiaoPorDDD(contato.DDD);
                if (regiao.Equals("DDD_INVALIDO"))
                {
                    throw new CustomException(HttpStatusCode.BadRequest, $"Região NÃO ENCONTRADA para o DDD: {contato.DDD}");
                }
                contato.Regiao = regiao;
            }

            ContatoEntity mapper = ContatoMapper.ToEntity(contato);

            await _contatosRepository.Atualizar(mapper);
        }
        public async Task ExcluirContato(int id)
        {
            await ValidaIdContato(id);
            await _contatosRepository.Excluir(id);
        }

        private async Task ValidaIdContato(int id)
        {
            if (id == 0)
                throw new CustomException(HttpStatusCode.BadRequest, "O id deve ser maior que zero.");
            if (!await _contatosRepository.ContatoExistePorId(id))
                throw new CustomException(HttpStatusCode.NotFound, $"O id do contato não existe.");
        }
    }
}
