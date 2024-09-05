#!/usr/bin/env python3

# cSpell:ignore jsonpatch

from os.path import exists, dirname, isdir
from os import access, W_OK
from typing import Any, Sequence
from jsonpatch import apply_patch # type: ignore
from json import dumps, loads
from pathlib import Path
from argparse import Namespace, ArgumentParser, Action

Unknown = Any
JSON = dict[str, Any] | Unknown | Any

class ValidateFile(Action):
    def __call__(self, parser: ArgumentParser, namespace: Namespace, values: str | Sequence[Any] | None, option_string: str | None = None) -> None:
        if values is None:
            parser.error(f"Please enter a valid path. Got: {values}")
        if isinstance(values, str):
            if not exists(values):
                parser.error(f"Please enter a valid path. Got: {values}")
            setattr(namespace, self.dest, Path(values))
        else:
            output: Sequence[Any] = []
            for value in values:
                if not exists(value):
                    parser.error(f"Please enter a valid path. Got: {value}")
                output.append(Path(value))
            setattr(namespace, self.dest, output)

def can_create_file(filename: str) -> bool:
    base_dir = dirname(filename)
    return not exists(filename) and isdir(base_dir) and access(base_dir, W_OK)

class ValidateWriteFile(Action):
    def __call__(self, parser: ArgumentParser, namespace: Namespace, values: str | Sequence[Any] | None, option_string: str | None = None) -> None:
        if values is None:
            parser.error(f"Please enter a valid path. Got: {values}")
        if isinstance(values, str):
            if not can_create_file(values):
                parser.error(f"Please enter a valid path. Got: {values}")
            setattr(namespace, self.dest, Path(values))
        else:
            output: Sequence[Any] = []
            for value in values:
                if not can_create_file(value):
                    parser.error(f"Please enter a valid path. Got: {value}")
                output.append(Path(value))
            setattr(namespace, self.dest, output)

def cli() -> None:
    parser: ArgumentParser = ArgumentParser(prog="patch_json.exe")
    parser.add_argument("input", type=str, action=ValidateFile, help="The input file to patch.")
    parser.add_argument("output", type=str, action=ValidateWriteFile, help="The output file to write the patched file to.")
    parser.add_argument("-p", "--patch", dest="patches", type=str, action=ValidateFile, nargs="+", required=True, help="The output file to write the patched file to.")
    args: Namespace = parser.parse_args()

    input: Path = args.input
    output: Path = args.output
    patches: list[Path] = args.patches

    content: str
    with input.open(mode="r", encoding="utf8") as fr:
        content = fr.read()

    json_content: JSON = loads(content)

    total_patches = len(patches)
    for index, patch in enumerate(patches):
        print(f"applying patch {index + 1}/{total_patches}")
        patch_content: str
        with patch.open(mode="r", encoding="utf8") as fpr:
            patch_content = fpr.read()
        json_patch_content: JSON = loads(patch_content)
        json_content = apply_patch(json_content, json_patch_content) # type: ignore

    content = dumps(json_content, indent=2)

    with output.open(mode="w", encoding="utf8") as fw:
        fw.write(content)

if __name__ == "__main__":
    cli()
