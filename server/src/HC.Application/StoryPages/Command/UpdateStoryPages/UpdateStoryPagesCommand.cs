﻿using HC.Application.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;

namespace HC.Application.StoryPages.Command.CreateStoryPages;

public class UpdateStoryPagesCommand : IRequest<BaseResult>
{
    public Guid StoryId { get; set; }
    public IEnumerable<string> Contents { get; set; }
}