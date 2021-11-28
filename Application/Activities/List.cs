 using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Activities
{
    public class List
    {
        public class Query : IRequest<Result<List<ActivityDto>>> { }

        public class Handler : IRequestHandler<Query, Result<List<ActivityDto>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<List<ActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                //egarly loading
                //var acitities = await _context.Activities
                //    .Include(a => a.Attendees) //get attendees
                //    .ThenInclude(u => u.AppUser) //then get app user
                //    .ToListAsync();
                //        var activitiesToReturn = _mapper.Map<List<ActivityDto>>(acitities);
                //return Result<List<ActivityDto>>.Sucess(acitities);

                //projection => chi query nhung cai minh muon, nen co the process nhanh hon, selecting sepecificly
                //project to dto luon voi mapping profile da co san ben class MappingProfiles
                var acitities = await _context.Activities
                    .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                return Result<List<ActivityDto>>.Sucess(acitities);
            }
        }
    }
}
