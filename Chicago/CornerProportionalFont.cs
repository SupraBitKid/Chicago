﻿using System;
using System.Collections.Generic;
using FontUtility;

public class CornerProportionalFont : ProportionalFontBase {
	private static Dictionary<char, byte[][]> characterLookup;

	public CornerProportionalFont() {
		this.Height = 8;
		this.Spacing = 5;
		this.fontCharacters = CornerProportionalFont.characterLookup;
	}

	static CornerProportionalFont() {
		CornerProportionalFont.characterLookup = new Dictionary<char, byte[][]> {
				//OOOOO
				//OOO..
				//OO...
				//O....
				//O....
				//.....
				//.....
				//.....
			{ '1', new byte[][] {
				new byte[] { 0b00011111, 0b00000111, 0b00000011, 0b00000001, 0b00000001 }
			}},
				//OOOOO
				//..OOO
				//...OO
				//....O
				//....O
				//.....
				//.....
				//.....
			{ '2', new byte[][] {
				new byte[] { 0b00000001, 0b00000001, 0b00000011, 0b00000111, 0b00011111 }
			}},
				//.....
				//.....
				//.....
				//O....
				//O....
				//OO...
				//OOO..
				//OOOOO
			{ '3', new byte[][] {
				new byte[] { 0b11111000, 0b11100000, 0b11000000, 0b10000000, 0b10000000 },
			}},
				//.....
				//.....
				//.....
				//....O
				//....O
				//...OO
				//..OOO
				//OOOOO
			{ '4', new byte[][] {
				new byte[] { 0b10000000, 0b10000000, 0b11000000, 0b11100000, 0b11111000 }
			}},
				//OOOO
				//OOO.
				//OO..
				//O...
				//....
				//....
				//....
				//....
			{
				'A', new byte[][] {
				new byte[] { 0b00001111, 0b00000111, 0b00000011, 0b00000001 }
			}},
				//OOOO
				//.OOO
				//..OO
				//...O
				//....
				//....
				//....
				//....
			{
				'B', new byte[][] {
				new byte[] { 0b00000001, 0b00000011, 0b00000111, 0b00001111 }
			}},
				//....
				//....
				//....
				//....
				//O...
				//OO..
				//OOO.
				//OOOO
			{
				'C', new byte[][] {
				new byte[] { 0b11110000, 0b11100000, 0b11000000, 0b10000000 },
			}},
				//....
				//....
				//....
				//....
				//...O
				//..OO
				//.OOO
				//OOOO
			{
				'D', new byte[][] {
				new byte[] { 0b10000000, 0b11000000, 0b11100000, 0b11110000 }
			}},
				//OOOO
				//OO..
				//O..
				//O...
				//....
				//....
				//....
				//....
			{
				'a', new byte[][] {
				new byte[] { 0b00001111, 0b00000011, 0b00000001, 0b00000001 }
			}},
				//OOOO
				//..OO
				//...O
				//...O
				//....
				//....
				//....
				//....
			{
				'b', new byte[][] {
				new byte[] { 0b00000001, 0b00000001, 0b00000011, 0b00001111 }
			}},
				//....
				//....
				//....
				//....
				//O...
				//O...
				//OO..
				//OOOO
			{
				'c', new byte[][] {
				new byte[] { 0b11110000, 0b11000000, 0b10000000, 0b10000000 },
			}},
				//....
				//....
				//....
				//....
				//...O
				//...O
				//..OO
				//OOOO
			{
				'd', new byte[][] {
				new byte[] { 0b10000000, 0b10000000, 0b11000000, 0b11110000 }
			}}
		};
	}
}