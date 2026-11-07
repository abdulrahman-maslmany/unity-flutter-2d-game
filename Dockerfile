# Base image containing .NET 8 SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS runner

# Install Linux dependencies for Flutter
RUN apt-get update && apt-get install -y --no-install-recommends \
    curl git unzip xz-utils libglu1-mesa \
    && rm -rf /var/lib/apt/lists/*

# Install Flutter SDK (Stable)
ENV FLUTTER_HOME=/opt/flutter
ENV PATH="$FLUTTER_HOME/bin:$PATH"

RUN git clone --depth 1 -b stable https://github.com/flutter/flutter.git $FLUTTER_HOME \
    && flutter precache --linux \
    && git config --global --add safe.directory '*'

WORKDIR /app

# Copy project files
COPY . .

# Run both .NET Game tests and Flutter tests
CMD sh -c "dotnet test Assets/Tests/GameTests.csproj --verbosity normal && cd my_app && flutter test"
