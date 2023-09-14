using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace BSChallenger.Server.API.Authentication
{
	[ApiController]
	[Route("[controller]")]
	public class ModAuthController : ControllerBase
	{
		private readonly byte[] _authPage = System.IO.File.ReadAllBytes(Path.Combine(Environment.CurrentDirectory, "HTML/ModAuth.html"));
		[HttpGet("/mod-auth")]
		public FileContentResult Index()
		{
			return File(_authPage, "text/html");
		}
	}
}
