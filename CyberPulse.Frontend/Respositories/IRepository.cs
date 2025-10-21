﻿using CyberPulse.Shared.EntitiesDTO.Chipp.Report;

namespace CyberPulse.Frontend.Respositories;

public interface IRepository
{
    Task<HttpResponseWrapper<byte[]>> GetBytesAsync(string url);
    Task<HttpResponseWrapper<byte[]>> GetBytesAsync(string url, ChipReport model);

    Task<HttpResponseWrapper<T>> GetAsync<T>(string url);

    Task<HttpResponseWrapper<object>> GetAsync(string url);

    Task<HttpResponseWrapper<object>> PostAsync<T>(string url, T model);

    Task<HttpResponseWrapper<TActionResponse>> PostAsync<T, TActionResponse>(string url, T model);

    Task<HttpResponseWrapper<object>> DeleteAsync(string url);

    Task<HttpResponseWrapper<object>> PutAsync<T>(string url, T model);

    Task<HttpResponseWrapper<TActionResponse>> PutAsync<T, TActionResponse>(string url, T model);

}