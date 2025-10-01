import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react'
import { apiBaseUrl } from '../../env';
import type { ServiceResponse } from '../../services/types';
import type { Game } from '../../pages/gamePage/types';

export const gameApi = createApi({
    reducerPath: 'game',
    baseQuery: fetchBaseQuery({
        baseUrl: `${apiBaseUrl}/game`
    }),
    tagTypes: ['Games'],
    endpoints: (build) => ({
        getGames: build.query<ServiceResponse<Game[]>, null>({
            query: () => ({ url: '/', method: "get" }),
            providesTags: ['Games']
        })
    })
});


export const { useGetGamesQuery } = gameApi;