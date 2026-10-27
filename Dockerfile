FROM ghcr.io/cirruslabs/flutter:3.24.0

WORKDIR /app

# Copy dependency manifests
COPY my_app/pubspec*.yaml ./my_app/
RUN cd my_app && flutter pub get

# Copy full application codebase
COPY . .

# Run static analysis and automated test suites
CMD ["sh", "-c", "cd my_app && flutter analyze && flutter test"]
