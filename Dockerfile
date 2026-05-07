FROM ubuntu:20.04

WORKDIR /app

COPY ./Build/ .

ENV GAME_DIFFICULTY=Normal
ENV GAME_DATA_PATH=/app/Island_of_Eshtein_Data

CMD ["echo", "Unity Build packed successfully. Run 'Island of Eshtein.exe' on Windows."]