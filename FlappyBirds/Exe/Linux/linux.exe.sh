#!/bin/sh
echo -ne '\033c\033]0;FlappyBirds\a'
base_path="$(dirname "$(realpath "$0")")"
"$base_path/linux.exe.x86_64" "$@"
