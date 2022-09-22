using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HyperDemonInfo.Server.Services;

public class LeaderboardProxy
{
	private readonly HttpClient _httpClient;

	public LeaderboardProxy(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<string> GetLeaderboardAsync(int offset)
	{
		List<KeyValuePair<string, string>> parameters = new()
		{
			new("offset", offset.ToString()),
		};
		using FormUrlEncodedContent content = new(parameters);
		HttpResponseMessage response = await _httpClient.PostAsync("http://hyprd.mn/backend_dev/get_scores.php", content);
		byte[] bytes = await response.Content.ReadAsByteArrayAsync();

		using MemoryStream ms = new(bytes);
		using BinaryReader br = new(ms);
		byte[] header = br.ReadBytes(7); // HD_GSC1
		_ = br.ReadInt32();
		int count = br.ReadInt32(); // 100 results are returned when offset is 0, 500 otherwise
		_ = br.ReadInt32();
		_ = br.ReadInt32();
		_ = br.ReadInt32();
		_ = br.ReadSingle();

		List<Entry> entries = new();
		for (int i = 0; i < count; i++)
		{
			int rank = br.ReadInt32();
			int id = br.ReadInt32();
			int unknown = br.ReadInt32();
			int time = br.ReadInt32();
			int timestamp = br.ReadInt32();

			DateTime dateTime = DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime;
			entries.Add(new(rank, id, unknown, time / 10_000f, dateTime));
		}

		byte[] end = br.ReadBytes(16);

		Leaderboard lb = new(count, offset, entries);
		return JsonSerializer.Serialize(lb, new JsonSerializerOptions
		{
			NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals,
			WriteIndented = true,
		});
	}

	public record Leaderboard(int TotalResults, int Offset, List<Entry> Entries);

	public record Entry(int Rank, int PlayerId, int Unknown, float Time, DateTime DateTime);
}
